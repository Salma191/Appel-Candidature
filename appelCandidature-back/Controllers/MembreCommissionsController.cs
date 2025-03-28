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
    public class MembreCommissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembreCommissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MembreCommissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembreCommission>>> GetMembreCommissions()
        {
            return await _context.MembreCommissions.ToListAsync();
        }

        // GET: api/MembreCommissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MembreCommission>> GetMembreCommission(int id)
        {
            var membreCommission = await _context.MembreCommissions.FindAsync(id);

            if (membreCommission == null)
            {
                return NotFound();
            }

            return membreCommission;
        }

        [HttpGet("role")]
        public ActionResult<string> GetRoleCommission()
        {
            var roles = Enum.GetNames(typeof(RoleCommission));
            return Ok(roles);
        }

        // PUT: api/MembreCommissions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembreCommission(int id, MembreCommission membreCommission)
        {
            if (id != membreCommission.Id)
            {
                return BadRequest();
            }

            _context.Entry(membreCommission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembreCommissionExists(id))
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

        // POST: api/MembreCommissions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MembreCommission>> PostMembreCommission(MembreCommission membreCommission)
        {
            _context.MembreCommissions.Add(membreCommission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMembreCommission", new { id = membreCommission.Id }, membreCommission);
        }

        // DELETE: api/MembreCommissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembreCommission(int id)
        {
            var membreCommission = await _context.MembreCommissions.FindAsync(id);
            if (membreCommission == null)
            {
                return NotFound();
            }

            _context.MembreCommissions.Remove(membreCommission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MembreCommissionExists(int id)
        {
            return _context.MembreCommissions.Any(e => e.Id == id);
        }
    }
}
