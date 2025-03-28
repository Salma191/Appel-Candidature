using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.DTOs;
using pfe_back.Models;

namespace pfe_back.Controllers
{
    [Authorize(Roles = "DAO")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommissionsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Commissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commission>>> GetCommissions()
        {
            return await _context.Commissions
                .Include(u => u.MembreCommissions)
                .ToListAsync();
        }

        // GET: api/Commissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Commission>> GetCommission(int id)
        {
            var commission = await _context.Commissions.FindAsync(id);

            if (commission == null)
            {
                return NotFound();
            }

            return commission;
        }

        // PUT: api/Commissions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommission(int id, Commission commission)
        {
            if (id != commission.Id)
            {
                return BadRequest();
            }

            _context.Entry(commission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommissionExists(id))
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


        [HttpPost]
        public async Task<IActionResult> CreateCommission(Commission commission)
        {
            if (commission == null)
            {
                return BadRequest(new { message = "Commission est null." });
            }

            if (commission.MembreCommissions == null || !commission.MembreCommissions.Any())
            {
                return BadRequest(new { message = "MembreCommissions est null ou vide." });
            }

            // Création de la commission sans l'ID (généré après SaveChangesAsync)
            var commiss = new Commission
            {
                Nom = "Commission temporaire",
                MembreCommissions = new List<MembreCommission>()
            };

            // Récupération des utilisateurs
            var utilisateurIds = commission.MembreCommissions.Select(mc => mc.UtilisateurId).ToList();
            var utilisateurs = await _context.Utilisateurs
                .Where(u => utilisateurIds.Contains(u.Id))
                .ToListAsync();


            // Ajout ou mise à jour des membres
            foreach (var membreDto in commission.MembreCommissions)
            {
                var utilisateur = utilisateurs.FirstOrDefault(u => u.Id == membreDto.UtilisateurId);
                if (utilisateur == null)
                {
                    return NotFound(new { message = $"Utilisateur avec ID {membreDto.UtilisateurId} non trouvé." });
                }

                    commiss.MembreCommissions.Add(new MembreCommission
                    {
                        UtilisateurId = utilisateur.Id,
                        RoleCommission = membreDto.RoleCommission,
                        CommissionId = commiss.Id,
                        Utilisateur = utilisateur,
                        Commission = commiss
                    });

                    utilisateur.Role = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "Membre de commission");
            }

            await _context.Commissions.AddAsync(commiss);
            await _context.SaveChangesAsync(); // L'ID est maintenant généré

            // Mise à jour du nom avec l'ID
            commiss.Nom = $"Commission {commiss.Id}";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Commission créée avec succès.", commissionId = commiss.Id });
        }



        // DELETE: api/Commissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommission(int id)
        {
            var commission = await _context.Commissions.FindAsync(id);
            if (commission == null)
            {
                return NotFound();
            }

            _context.Commissions.Remove(commission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommissionExists(int id)
        {
            return _context.Commissions.Any(e => e.Id == id);
        }
    }
}
