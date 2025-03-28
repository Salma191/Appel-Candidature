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
    public class TypePostesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TypePostesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TypePostes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypePoste>>> GetTypePostes()
        {
            return await _context.TypePostes.ToListAsync();
        }

        // GET: api/TypePostes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypePoste>> GetTypePoste(int id)
        {
            var typePoste = await _context.TypePostes.FindAsync(id);

            if (typePoste == null)
            {
                return NotFound();
            }

            return typePoste;
        }

        // PUT: api/TypePostes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypePoste(int id, TypePoste typePoste)
        {
            if (id != typePoste.Id)
            {
                return BadRequest();
            }

            _context.Entry(typePoste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypePosteExists(id))
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

        // POST: api/TypePostes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TypePoste>> PostTypePoste(TypePoste typePoste)
        {
            _context.TypePostes.Add(typePoste);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypePoste", new { id = typePoste.Id }, typePoste);
        }

        // DELETE: api/TypePostes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypePoste(int id)
        {
            var typePoste = await _context.TypePostes.FindAsync(id);
            if (typePoste == null)
            {
                return NotFound();
            }

            _context.TypePostes.Remove(typePoste);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypePosteExists(int id)
        {
            return _context.TypePostes.Any(e => e.Id == id);
        }
    }
}
