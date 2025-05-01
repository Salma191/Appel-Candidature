using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;
using pfe_back.DTOs;
using pfe_back.Services;

namespace pfe_back.Controllers
{
    //[Authorize(Roles = "DAO")]
    [Route("api/[controller]")]
    [ApiController]
    public class DecisionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DecisionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Decisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Decision>>> GetDecision()
        {

            try
            {
                return await _context.Decisions
                    .Include(p => p.Entite)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
}

        [HttpGet("appel")]
        public async Task<ActionResult<IEnumerable<Decision>>> GetAppel()
        {
            var appel = await _context.Decision
                 .Include(d => d.Entite)
                .Include(d => d.Postes!)
                    .ThenInclude(d => d.TypePoste)
                .Where(d => d.Statut == "Lancée")
                .ToListAsync();

            return appel;
        }

        // GET: api/Decisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Decision>> GetDecision(int id)
        {
            var decision = await _context.Decisions
                .Include(d => d.Entite)
                .Include(d => d.Postes)
                    .ThenInclude(d => d.TypePoste)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (decision == null)
            {
                return NotFound();
            }

            return decision;
        }


        [HttpPost("signe/{decisionid}")]
        public async Task<ActionResult<PieceJointe>> AddPJSignee(int decisionid, [FromBody] PieceJointe pieceJointe)
        {
            // Vérifier si la décision existe
            var decision = await _context.Decisions.FirstOrDefaultAsync(d => d.Id == decisionid);
            if (decision == null)
            {
                return NotFound($"La décision avec l'ID {decisionid} n'a pas été trouvée.");
            }

            // Initialiser la liste des pièces jointes si elle est null
            if (decision.PieceJointes == null)
            {
                decision.PieceJointes = new List<PieceJointe>();
            }

            // Créer la nouvelle pièce jointe
            var newPieceJointe = new PieceJointe()
            {
                Nom = pieceJointe.Nom,
                Fichier = pieceJointe.Fichier,
                Type = TypePJ.Signée,
                DecisionId = decisionid
            };

            // Ajouter la pièce jointe à la liste des pièces jointes du PV
            decision.PieceJointes.Add(newPieceJointe);
            decision.Statut = "Signée";  // Le statut doit être "Signé"
            decision.DateSignature = DateTime.UtcNow;

            try
            {
                // Sauvegarder les modifications dans la base de données
                await _context.SaveChangesAsync();
                return Ok(newPieceJointe);  // Retourner la pièce jointe ajoutée
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de l'ajout de la pièce jointe : {ex.Message}");
            }
        }


        // PUT: api/Decisions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDecision(int id, DecisionDto decision)
            {
                var dec = await _context.Decisions
                    .Include(d => d.DecisionPhases)
                    .ThenInclude(dp => dp.Phase)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (dec == null)
                {
                    Console.WriteLine($"🚨 Décision {id} non trouvée !");
                    return NotFound();
                }

                Console.WriteLine($"🔄 Mise à jour de la décision {id} avec statut {dec.Statut}");

                if (dec.Statut == "Signée")
                {
                    dec.DatePublication = DateTime.UtcNow;
                    dec.DateLimite = decision.DateLimite;
                    Console.WriteLine($"📅 Date de publication définie : {dec.DatePublication}");

                    // Désactivation de la phase "Draft"
                    var draftPhase = dec.DecisionPhases.FirstOrDefault(p => p.Phase?.Nom == NomPhase.Draft);
                    if (draftPhase != null)
                    {
                        draftPhase.Statut = "Désactivé";
                        draftPhase.DateFin = DateTime.UtcNow;
                        _context.Entry(draftPhase).State = EntityState.Modified;
                        Console.WriteLine($"❌ Phase 'Draft' désactivée");
                    }

                    // Vérifier si la phase "Lancement" existe déjà
                    var lancementPhase = dec.DecisionPhases.FirstOrDefault(p => p.Phase?.Nom == NomPhase.Preselection);
                    if (lancementPhase == null)
                    {
                        // Vérifier si la phase "Lancement" existe dans la base
                        var existingPhase = await _context.Phases.FirstOrDefaultAsync(p => p.Nom == NomPhase.Preselection);

                        lancementPhase = new DecisionPhase
                        {
                            DateDebut = DateTime.UtcNow,
                            Statut = "Activé",
                            Phase = existingPhase ?? new Phase { Nom = NomPhase.Preselection }, // Réutiliser la phase existante si elle existe
                            DecisionId = dec.Id
                        };

                        dec.DecisionPhases.Add(lancementPhase); // Ajouter à la décision
                        await _context.DecisionPhases.AddAsync(lancementPhase);
                        Console.WriteLine($"✅ Phase 'Lancement' ajoutée");
                    }

                    // Mise à jour du statut de la décision
                    dec.Statut = "Lancée";
                    _context.Entry(dec).State = EntityState.Modified;

                    // Sauvegarde unique des changements
                    try
                    {
                        Console.WriteLine("💾 Sauvegarde des modifications...");
                        await _context.SaveChangesAsync();
                        Console.WriteLine("✅ Sauvegarde réussie !");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Console.WriteLine("⚠️ Erreur de mise à jour concurrentielle");
                        if (!DecisionExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return NoContent();
            }


        [HttpPut("entretienPhase/{id}")]
        public async Task<IActionResult> PutDecisionEnt(int id, DecisionDto decision)
        {
            var dec = await _context.Decisions
                .Include(d => d.DecisionPhases)
                .ThenInclude(dp => dp.Phase)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dec == null)
            {
                Console.WriteLine($"🚨 Décision {id} non trouvée !");
                return NotFound();
            }

            Console.WriteLine($"🔄 Mise à jour de la décision {id} avec statut {dec.Statut}");

            if (dec.Statut == "Lancée")
            {

                // Désactivation de la phase "Preselection"
                var preselectionPhase = dec.DecisionPhases.FirstOrDefault(p => p.Phase?.Nom == NomPhase.Preselection);
                if (preselectionPhase != null)
                {
                    preselectionPhase.Statut = "Désactivé";
                    preselectionPhase.DateFin = DateTime.UtcNow;
                    _context.Entry(preselectionPhase).State = EntityState.Modified;
                    Console.WriteLine($"❌ Phase 'Preselection' désactivée");
                }

                // Vérifier si la phase "Entretien" existe déjà
                var entretienPhase = dec.DecisionPhases.FirstOrDefault(p => p.Phase?.Nom == NomPhase.Entretien);
                if (entretienPhase == null)
                {
                    // Vérifier si la phase "Entretien" existe dans la base
                    var existingPhase = await _context.Phases.FirstOrDefaultAsync(p => p.Nom == NomPhase.Entretien);

                    entretienPhase = new DecisionPhase
                    {
                        DateDebut = DateTime.UtcNow,
                        Statut = "Activé",
                        Phase = existingPhase ?? new Phase { Nom = NomPhase.Entretien }, // Réutiliser la phase existante si elle existe
                        DecisionId = dec.Id
                    };

                    dec.DecisionPhases.Add(entretienPhase); // Ajouter à la décision
                    await _context.DecisionPhases.AddAsync(entretienPhase);
                    Console.WriteLine($"✅ Phase 'Entretien' ajoutée");
                }

                // Mise à jour du statut de la décision
                _context.Entry(dec).State = EntityState.Modified;

                // Sauvegarde unique des changements
                try
                {
                    Console.WriteLine("💾 Sauvegarde des modifications...");
                    await _context.SaveChangesAsync();
                    Console.WriteLine("✅ Sauvegarde réussie !");
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("⚠️ Erreur de mise à jour concurrentielle");
                    if (!DecisionExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }


        [HttpPost("pj/{decisionid}")]
        public async Task<ActionResult<PieceJointe>> AddPJ(int decisionid, [FromBody] PieceJointe pieceJointe)
        {
            // Vérifier si le PV existe
            var decision = await _context.Decisions.FirstOrDefaultAsync(d => d.Id == decisionid);
            if (decision == null)
            {
                return NotFound($"La decision avec l'ID {decisionid} n'a pas été trouvé.");
            }

            // Initialiser la liste des pièces jointes si elle est null
            if (decision.PieceJointes == null)
            {
                decision.PieceJointes = new List<PieceJointe>();
            }

            // Créer la nouvelle pièce jointe
            var newPieceJointe = new PieceJointe()
            {
                Nom = pieceJointe.Nom,
                Fichier = pieceJointe.Fichier,
                Type = TypePJ.Supp,
                DecisionId = decisionid
            };


            // Ajouter la pièce jointe à la liste des pièces jointes du PV
            decision.PieceJointes.Add(newPieceJointe);

            try
            {
                // Sauvegarder les modifications dans la base de données
                await _context.SaveChangesAsync();
                return Ok(newPieceJointe);  // Retourner la pièce jointe ajoutée
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de l'ajout de la pièce jointe : {ex.Message}");
            }
        }


        [HttpGet("pj/{id}")]
        public async Task<ActionResult<object>> GetPJ(int id)
        {
            var pjs = await _context.PieceJointes
                .Where(p => p.DecisionId == id)
                .Select(pj => new {pj.Id, pj.Nom, pj.Type })
                .ToListAsync();

            var result = new
            {
                Signees = pjs.Where(p => p.Type == TypePJ.Signée).Select(pj => new { pj.Id ,pj.Nom}).ToList(),
                Supp = pjs.Where(p => p.Type == TypePJ.Supp).Select(pj => new { pj.Id, pj.Nom }).ToList()
            };

            return Ok(result);
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


        [HttpGet("phase/{id}")]
        public async Task<ActionResult<string>> GetPhase(int id)
        {
            var decPhase = await _context.DecisionPhases
                .Include(p => p.Phase)
                .Where(p => p.DecisionId == id && p.Statut == "Activé")
                .FirstOrDefaultAsync();

            if (decPhase == null || decPhase.Phase == null)
            {
                return Ok(new { phase = "Décision en Draft" });
            }

            var phaseName = EnumUtils.GetEnumDisplayName(decPhase.Phase.Nom);

            return Ok(new { phase = phaseName });
        }


        // DELETE: api/Decisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDecision(int id)
        {
            var decision = await _context.Decisions.FindAsync(id);
            if (decision == null)
            {
                return NotFound();
            }

            _context.Decisions.Remove(decision);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DecisionExists(int id)
        {
            return _context.Decisions.Any(e => e.Id == id);
        }



    }

}
