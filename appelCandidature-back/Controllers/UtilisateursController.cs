using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;

namespace pfe_back.Controllers
{
    [Authorize(Roles = "Administrateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtilisateursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {

            return await _context.Utilisateurs.Include(u => u.Role).ToListAsync();
        }

        // GET: api/Utilisateurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateur(int id)
        {
            var utilisateur = await _context.Utilisateurs
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (utilisateur == null)
            {
                return NotFound("L'utilisateur n'existe pas.");
            }


            return utilisateur;
        }

        // PUT: api/Utilisateurs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {

            utilisateur.Id = id;
            utilisateur.Password = BCrypt.Net.BCrypt.HashPassword(utilisateur.Password);
            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Utilisateur modifié avec succès !");
        }

        // POST: api/Utilisateurs
        [HttpPost]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            var emailExists = await _context.Utilisateurs.AnyAsync(u => u.Email == utilisateur.Email);
            if(emailExists)
            {
                return Conflict(new { message = "Cet email est déjà utilisé." });
            }

            utilisateur.Role = await _context.Roles.FindAsync(utilisateur.RoleId);
            utilisateur.Password = BCrypt.Net.BCrypt.HashPassword(utilisateur.Password);

  
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();

            await _context.Entry(utilisateur).Reference(u => u.Role).LoadAsync();


            return CreatedAtAction("GetUtilisateur", new { id = utilisateur.Id }, utilisateur);
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }


            _context.Utilisateurs.Remove(utilisateur);
            await _context.SaveChangesAsync();

            return Ok("Utilisateur supprimé avec succès !");
        }

        private bool UtilisateurExists(int id)
        {
            return _context.Utilisateurs.Any(e => e.Id == id);
        }
    }
}
