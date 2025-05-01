using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;
using pfe_back.Services;

namespace pfe_back.Controllers
{
    //[Authorize(Roles = "DAO")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Postes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Poste>>> GetPoste()
        {
            return await _context.Postes
                .Include(p => p.Entite)
                .Include(p => p.TypePoste)
                .ToListAsync();
        }

        // GET: api/Postes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Poste>> GetPoste(int id)
        {
            var poste = await _context.Postes
                .Include(p => p.TypePoste)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (poste == null)
            {
                return NotFound();
            }

            return poste;
        }

        [HttpGet("entite/{entiteId}/type/{typeId}")]
        public async Task<ActionResult<IEnumerable<Poste>>> GetPosteByTypeAndEntite(int entiteId, int typeId)
        {
            var postes = await _context.Postes
                .Where(p => p.EntiteId == entiteId && p.TypePosteId == typeId)
                .ToListAsync();

            if (postes == null || !postes.Any())
            {
                return NotFound();
            }

            return Ok(postes);
        }

        [HttpGet("byDecision/{id}")]
        public async Task<ActionResult<IEnumerable<Poste>>> getPosteByDecision(int id)
        {
            var postes = await _context.Postes
                .Include(p => p.Decisions)
                .Include(p => p.Entite)
                .Include(p => p.TypePoste)
                .Include(p => p.Candidatures)
                .Where(p => p.Decisions.Any(d => d.Id == id))
                .ToListAsync();

            if (postes == null || !postes.Any())
            {
                return NotFound();
            }

            var result = new List<object>();

            foreach (var poste in postes)
            {
                result.Add(new
                {
                    poste.Id,
                    poste.NumeroUnique,
                    TypePosteNom = poste.TypePoste?.Nom,
                    EntiteNom = poste.Entite?.Nom,
                    Candidatures = poste.Candidatures?.Count()
                });
            }

            return Ok(result);
        }

        [HttpGet("appel")]
        public async Task<ActionResult<IEnumerable<object>>> getPosteActive()
        {
            var postes = await _context.Postes
                .Include(p => p.Decisions)
                .Include(p => p.Entite)
                .Include(p => p.TypePoste)
                .Include(p => p.Candidatures)
                .Where(p => p.Decisions.Any(d => d.Statut == "Lancée"))
                .ToListAsync();

            if (postes == null || !postes.Any())
            {
                return NotFound();
            }

            var result = new List<object>();

            foreach (var poste in postes)
            {

                var decisionLancee = poste.Decisions.FirstOrDefault(d => d.Statut == "Lancé");

                result.Add(new
                {
                    poste.Id,
                    poste.Description,
                    poste.Exigence,
                    poste.Critere,
                    poste.NumeroUnique,
                    DateLimite = decisionLancee?.DateLimite

                });
            }

            return Ok(result);
        }

        // PUT: api/Postes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoste(int id, Poste poste)
        {
            poste.Id = id;

            _context.Entry(poste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PosteExists(id))
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

        // POST: api/Postes
        [HttpPost]
        public async Task<IActionResult> AddPostes(List<Poste> postes)
        {
            if (postes == null || !postes.Any())
            {
                return BadRequest("La liste des postes est vide.");
            }

            _context.Postes.AddRange(postes);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Postes ajoutés avec succès." });
        }


        // DELETE: api/Postes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoste(int id)
        {
            var poste = await _context.Postes.FindAsync(id);
            if (poste == null)
            {
                return NotFound();
            }

            _context.Postes.Remove(poste);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PosteExists(int id)
        {
            return _context.Postes.Any(e => e.Id == id);
        }
    }
}