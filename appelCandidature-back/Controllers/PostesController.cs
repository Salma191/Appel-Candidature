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
            return await _context.Postes.ToListAsync();
        }

        // GET: api/Postes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Poste>> GetPoste(int id)
        {
            var poste = await _context.Postes.FindAsync(id);

            if (poste == null)
            {
                return NotFound();
            }

            return poste;
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
        public async Task<ActionResult<Poste>> PostPoste(Poste poste)
        {
            _context.Postes.Add(poste);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoste", new { id = poste.Id }, poste);
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