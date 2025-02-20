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
            return await _context.PVs.ToListAsync();
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
