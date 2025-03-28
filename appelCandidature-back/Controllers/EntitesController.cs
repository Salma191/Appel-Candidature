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
    public class EntitesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EntitesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Entites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entite>>> GetEntites()
        {
            return await _context.Entites.ToListAsync();
        }

        // GET: api/Entites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entite>> GetEntite(int id)
        {
            var entite = await _context.Entites.FindAsync(id);

            if (entite == null)
            {
                return NotFound();
            }

            return entite;
        }

        // PUT: api/Entites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntite(int id, Entite entite)
        {
            entite.Id = id;
            if (id != entite.Id)
            {
                return BadRequest();
            }

            _context.Entry(entite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntiteExists(id))
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

        // POST: api/Entites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Entite>> PostEntite(Entite entite)
        {
            _context.Entites.Add(entite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntite", new { id = entite.Id }, entite);
        }

        // DELETE: api/Entites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntite(int id)
        {
            var entite = await _context.Entites.FindAsync(id);
            if (entite == null)
            {
                return NotFound();
            }

            _context.Entites.Remove(entite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntiteExists(int id)
        {
            return _context.Entites.Any(e => e.Id == id);
        }
    }
}
