using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;

namespace pfe_back.Controllers
{
    [Authorize(Roles = "DAO")]
    [Route("api/[controller]")]
    [ApiController]
    public class PVsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PVsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PVs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PV>>> GetPV()
        {
            var pvs = await _context.PVs
                .Include(p => p.TypePoste)
                .Include(p => p.Commission)
                .ToListAsync();
            if (pvs == null || pvs.Count == 0)
            {
                return NotFound();
            }

            // Renvoyer uniquement les propriétés nécessaires, incluant le nom du poste et du comité
            var result = pvs.Select(pv => new
            {
                pv.Id,
                pv.Entite,
                pv.DateCreation,
                pv.Statut,
                TypePoste = pv.TypePoste?.Nom, // Assurer que le nom du poste est renvoyé
                Commission = pv.Commission?.Nom, // Assurer que le nom du comité est renvoyé
            }).ToList();

            return Ok(result);
        }

        // GET: api/PVs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PV>> GetPV(int id)
        {
            var pV = await _context.PVs.FindAsync(id);

            if (pV == null)
            {
                return NotFound();
            }

            return pV;
        }

        // PUT: api/PVs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPV(int id, PV pV)
        {
            pV.Id = id;

            _context.Entry(pV).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PVExists(id))
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

        // POST: api/PVs
        [HttpPost]
        public async Task<ActionResult<PV>> PostPV(PV pV)
        {

                // Vérification de l'utilisateur connecté
                var utilisateurConnecte = await _context.Utilisateurs
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

                if (utilisateurConnecte == null || utilisateurConnecte.Role?.Nom != "DAO")
                {
                    return BadRequest("Seuls les DAO peuvent créer un PV.");
                }

                // Vérification du TypePoste
                var typePoste = await _context.TypePostes.FindAsync(pV.TypePosteId);
                if (typePoste == null)
                {
                    return BadRequest("Type de poste invalide.");
                }

                pV.DAOId = utilisateurConnecte.Id;

                // Vérification de la commission et de ses membres
                if (pV.Commission == null || pV.Commission.MembreCommissions == null || pV.Commission.MembreCommissions.Count != 4)
                {
                    return BadRequest("La commission doit avoir exactement 4 membres.");
                }

                var membresIds = pV.Commission.MembreCommissions.Select(m => m.Id).ToList();
                var membres = await _context.Utilisateurs
                    .Where(u => membresIds.Contains(u.Id))
                    .ToListAsync();

                if (membres.Count != 4)
                {
                    return BadRequest("Certains membres de la commission sont invalides.");
                }

                // Assurer que chaque membre a un rôle valide
                foreach (var membreCommission in pV.Commission.MembreCommissions)
                {
                    var membre = membres.FirstOrDefault(m => m.Id == membreCommission.Id);
                    if (membre?.Role == null)
                    {
                        var roleMembre = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "Membre de commission");
                        if (roleMembre == null)
                        {
                            return BadRequest("Le rôle 'Membre de commission' n'existe pas.");
                        }
                        membre.RoleId = roleMembre.Id;
                    }
                }
                await _context.SaveChangesAsync();

                // Création de la commission
                var commission = new Commission
                {
                    Nom = pV.Commission.Nom,
                    MembreCommissions = pV.Commission.MembreCommissions,
                    PVs = new List<PV>()
                };

                _context.Commissions.Add(commission);
                await _context.SaveChangesAsync();

                // Association de la commission au PV
                pV.CommissionId = commission.Id;
                pV.DateCreation = DateTime.UtcNow;
                pV.Statut = "Draft";

                _context.PVs.Add(pV);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPV", new { id = pV.Id }, pV);

            }

        // DELETE: api/PVs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePV(int id)
        {
            var pV = await _context.PVs.FindAsync(id);
            if (pV == null)
            {
                return NotFound();
            }

            _context.PVs.Remove(pV);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PVExists(int id)
        {
            return _context.PVs.Any(e => e.Id == id);
        }
    }
}
