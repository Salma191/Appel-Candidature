using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;

namespace pfe_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidaturesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CandidaturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Candidatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidature>>> GetCandidatures()
        {
            return await _context.Candidatures.ToListAsync();
        }

        [HttpGet("mes-candidatures")]
        public async Task<ActionResult<IEnumerable<Candidature>>> GetMyCanditures()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "candidatId");
            if (userIdClaim == null)
                return Unauthorized("Candidat non identifié");

            int candidatId = int.Parse(userIdClaim.Value);

            var candidatures = await _context.Candidatures
                .Include(c => c.Poste)
                .Include(c => c.Decision)
                .Where(c => c.CandidatId == candidatId)
                .ToListAsync();

            return Ok(candidatures);
        }

        // GET: api/Candidatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidature>> GetCandidature(int id)
        {
            var candidature = await _context.Candidatures.FindAsync(id);

            if (candidature == null)
            {
                return NotFound();
            }

            return candidature;
        }

        [HttpGet("poste/{id}")]
        public async Task<IActionResult> GetCandidatureByPoste(int id)
        {
            var data = await _context.Candidatures
                .Include(c => c.Candidat)
                    .ThenInclude(c => c.Experiences)
                .Include(c => c.Candidat)
                    .ThenInclude(c => c.Diplomes)
              .Include(c => c.Candidat)
                .ThenInclude(c => c.Utilisateur)
              .Where(c => c.PosteId == id)
              .ToListAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            return new JsonResult(data, options);
        }


        // PUT: api/Candidatures/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCandidature(int id, Candidature candidature)
        {
            if (id != candidature.Id)
            {
                return BadRequest();
            }

            _context.Entry(candidature).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidatureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Candidatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidature(int id)
        {
            var candidature = await _context.Candidatures.FindAsync(id);
            if (candidature == null)
            {
                return NotFound();
            }

            _context.Candidatures.Remove(candidature);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("demarrer")]
        public async Task<IActionResult> Postuler([FromBody] Candidature dto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "candidatId");
            if (userIdClaim == null)
                return Unauthorized("Candidat non identifié");

            int candidatId = int.Parse(userIdClaim.Value);

            var candidat = await _context.Candidats.FindAsync(candidatId);
            var poste = await _context.Postes.FindAsync(dto.PosteId);
            var decision = await _context.Decisions.FindAsync(dto.DecisionId);

            if (candidat == null || poste == null || decision == null)
            {
                return NotFound("Candidat, poste ou décision introuvable.");
            }

            bool dejaPostule = await _context.Candidatures.AnyAsync(c =>
                c.CandidatId == candidatId && c.PosteId == dto.PosteId);

            if (dejaPostule)
            {
                return BadRequest("Une candidature existe déjà pour ce poste.");
            }

            int candidaturesSurCetteDecision = await _context.Candidatures
                            .CountAsync(c => c.CandidatId == candidatId && c.DecisionId == dto.DecisionId);

            if (candidaturesSurCetteDecision > decision.NbreMaxPosteCandidat)
            {
                return BadRequest($"Vous avez déjà postulé à {candidaturesSurCetteDecision} poste(s) dans cette décision. Limite autorisée : {decision.NbreMaxPosteCandidat}.");
            }

                var candidature = new Candidature
            {
                Statut = "En Brouillon",
                DatePostulation = DateTime.Now,
                CandidatId = dto.CandidatId,
                Candidat = candidat,
                PosteId = dto.PosteId,
                Poste = poste,
                DecisionId = dto.DecisionId,
                Decision = decision,
                EstComplete = false,
            };

            _context.Candidatures.Add(candidature);
            await _context.SaveChangesAsync();

            candidature.NumeroEnregistrement = $"CAND-{DateTime.Now.Year}-{candidature.Id:D4}";

            _context.Candidatures.Update(candidature);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Candidature brouillon créée.",
                candidatureId = candidature.Id,
                numeroEnregistrement = candidature.NumeroEnregistrement
            });
        }

        [HttpPut("{id}/finaliser")]
        public async Task<IActionResult> FinaliserCandidature(int id)
        {
            var candidature = await _context.Candidatures
                .Include(c => c.PieceJointes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidature == null)
                return NotFound("Candidature introuvable.");

            var hasCV = candidature.PieceJointes?.Any(p => p.TypeDoc == TypeDoc.CV) == true;
            var hasLettre = candidature.PieceJointes?.Any(p => p.TypeDoc == TypeDoc.CL) == true;

            if (!hasCV || !hasLettre)
                return BadRequest(new { message = "CV ou lettre de motivation manquants." });

            candidature.Statut = "En attente";
            candidature.EstComplete = true;

            _context.Candidatures.Update(candidature);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Candidature finalisée avec succès." });
        }

        [HttpPost("{candidatureid}/pj")]
        public async Task<ActionResult<PieceJointe>> AddPJ(int candidatureid, [FromBody] PieceJointe pieceJointe)
        {
            // Vérifier si le PV existe
            var candidature = await _context.Candidatures.FirstOrDefaultAsync(p => p.Id == candidatureid);
            if (candidature == null)
            {
                return NotFound($"La candidature avec l'ID {candidatureid} n'a pas été trouvé.");
            }

            if (candidature.PieceJointes == null)
            {
                candidature.PieceJointes = new List<PieceJointe>();
            }

            // Créer la nouvelle pièce jointe
            var newPieceJointe = new PieceJointe()
            {
                Nom = pieceJointe.Nom,
                Fichier = pieceJointe.Fichier,
                Type = TypePJ.Candidature,
                TypeDoc = pieceJointe.TypeDoc,
                CandidatureId = candidatureid
            };


            candidature.PieceJointes.Add(newPieceJointe);
            Console.WriteLine($"Nouvelle PJ ajoutée : Nom = {newPieceJointe.Nom}, candidatureId = {newPieceJointe.CandidatureId}");

            try
            {
                await _context.SaveChangesAsync();
                return Ok(newPieceJointe); 
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de l'ajout de la pièce jointe : {ex.Message}");
            }
        }

        [HttpGet("{id}/pj")]
        public async Task<ActionResult<IEnumerable<PieceJointe>>> GetCandidaturePJ(int id)
        {
            var candidature = await _context.Candidatures
                .Include(c => c.PieceJointes)
                .Where(c => c.Id == id)
                .Select(p => p.PieceJointes)
                .ToListAsync();

            return Ok(candidature);
        }

        [HttpGet("pj/preview/{id}")]
        public async Task<IActionResult> PreviewPJ(int id)
        {
            var pj = await _context.PieceJointes
                       .Where(p => p.Id == id)
                       .Select(p => new { p.Nom, p.Fichier })
                       .FirstOrDefaultAsync();

            if (pj == null)
                return NotFound();

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(pj.Fichier);
            }
            catch
            {
                return BadRequest("Invalid Base64 data");
            }

            var mime = "application/pdf";

            return File(bytes, mime, fileDownloadName: pj.Nom, enableRangeProcessing: true);
        }

        private bool CandidatureExists(int id)
        {
            return _context.Candidatures.Any(e => e.Id == id);
        }
    }
}
