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
    public class OrganigrammesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrganigrammesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Organigrammes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organigramme>>> GetOrganigramme()
        {
            return await _context.Organigramme.ToListAsync();
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

        // GET: api/Organigrammes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organigramme>> GetOrganigramme(int id)
        {
            var organigramme = await _context.Organigramme.FindAsync(id);

            if (organigramme == null)
            {
                return NotFound();
            }

            return organigramme;
        }

        // PUT: api/Organigrammes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganigramme(int id, Organigramme organigramme)
        {
            if (id != organigramme.Id)
            {
                return BadRequest();
            }

            _context.Entry(organigramme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganigrammeExists(id))
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

        // POST: api/Organigrammes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Organigramme>> PostOrganigramme(Organigramme organigramme)
        {
            _context.Organigramme.Add(organigramme);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganigramme", new { id = organigramme.Id }, organigramme);
        }

        // DELETE: api/Organigrammes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganigramme(int id)
        {
            var organigramme = await _context.Organigramme.FindAsync(id);
            if (organigramme == null)
            {
                return NotFound();
            }

            _context.Organigramme.Remove(organigramme);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganigrammeExists(int id)
        {
            return _context.Organigramme.Any(e => e.Id == id);
        }
    }
}
