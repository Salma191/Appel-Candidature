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
    public class DiplomesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiplomesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Diplomes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diplome>>> GetDiplomes()
        {
            return await _context.Diplomes.ToListAsync();
        }

        // GET: api/Diplomes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Diplome>> GetDiplome(int id)
        {
            var diplome = await _context.Diplomes.FindAsync(id);

            if (diplome == null)
            {
                return NotFound();
            }

            return diplome;
        }

        // PUT: api/Diplomes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiplome(int id, Diplome diplome)
        {
            if (id != diplome.Id)
            {
                return BadRequest();
            }

            _context.Entry(diplome).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiplomeExists(id))
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

        // POST: api/Diplomes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Diplome>> PostDiplome(Diplome diplome)
        {
            _context.Diplomes.Add(diplome);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDiplome", new { id = diplome.Id }, diplome);
        }

        // DELETE: api/Diplomes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiplome(int id)
        {
            var diplome = await _context.Diplomes.FindAsync(id);
            if (diplome == null)
            {
                return NotFound();
            }

            _context.Diplomes.Remove(diplome);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DiplomeExists(int id)
        {
            return _context.Diplomes.Any(e => e.Id == id);
        }
    }
}
