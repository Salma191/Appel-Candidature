using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CandidatsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CandidatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Candidats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidat>>> GetCandidats()
        {
            return await _context.Candidats.ToListAsync();
        }

        // GET: api/Candidats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidat>> GetCandidat(int id)
        {
            var candidat = await _context.Candidats.FindAsync(id);

            if (candidat == null)
            {
                return NotFound();
            }

            return candidat;
        }

        // PUT: api/Candidats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update-candidat/{userId}")]
        public async Task<IActionResult> PutCandidat(int userId, Candidat candidatModel)
        {
            var utilisateur = await _context.Utilisateurs.Include(u => u.Candidat)
                                                          .FirstOrDefaultAsync(u => u.Id == userId);
            if (utilisateur == null)
                return NotFound("Utilisateur non trouvé.");

            // Vérifier si l'utilisateur a déjà un candidat associé
            if (utilisateur.Candidat == null)
            {
                return BadRequest("Le Candidat n'existe pas.");
            }

            // Mise à jour des informations du candidat
            utilisateur.Candidat.AffectationActuelle = candidatModel.AffectationActuelle;
            utilisateur.Candidat.DateRetraite = candidatModel.DateRetraite;
            utilisateur.Candidat.JoursAbsence = candidatModel.JoursAbsence;
            utilisateur.Candidat.Sanction = candidatModel.Sanction;
            utilisateur.Candidat.NoteTroisDernieresAnnees = candidatModel.NoteTroisDernieresAnnees;
            utilisateur.Candidat.Catégorie = candidatModel.Catégorie;
            utilisateur.Candidat.Congé = candidatModel.Congé;
            utilisateur.Candidat.PosteOccupe = candidatModel.PosteOccupe;
            utilisateur.Candidat.Consentement = candidatModel.Consentement;

            // Sauvegarder les modifications dans la base de données
            await _context.SaveChangesAsync();

            return Ok("Informations du candidat mises à jour avec succès.");
        }



        // POST: api/Candidats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Candidat>> PostCandidat(Candidat candidat)
        {
            _context.Candidats.Add(candidat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCandidat", new { id = candidat.Id }, candidat);
        }

        // DELETE: api/Candidats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidat(int id)
        {
            var candidat = await _context.Candidats.FindAsync(id);
            if (candidat == null)
            {
                return NotFound();
            }

            _context.Candidats.Remove(candidat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CandidatExists(int id)
        {
            return _context.Candidats.Any(e => e.Id == id);
        }
    }
}
