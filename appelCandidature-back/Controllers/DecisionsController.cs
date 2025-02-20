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
    public class DecisionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DecisionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Decisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Decision>>> GetDecision()
        {
            return await _context.Decisions.ToListAsync();
        }

        // GET: api/Decisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Decision>> GetDecision(int id)
        {
            var decision = await _context.Decisions.FindAsync(id);

            if (decision == null)
            {
                return NotFound();
            }

            return decision;
        }

        // PUT: api/Decisions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDecision(int id, Decision decision)
        {
            decision.Id = id;

            _context.Entry(decision).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DecisionExists(id))
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

        // POST: api/Decisions
        [HttpPost]
        public async Task<ActionResult<Decision>> PostDecision(Decision decision)
        {
            _context.Decisions.Add(decision);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDecision", new { id = decision.Id }, decision);
        }

        // DELETE: api/Decisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDecision(int id)
        {
            var decision = await _context.Decisions.FindAsync(id);
            if (decision == null)
            {
                return NotFound();
            }

            _context.Decisions.Remove(decision);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DecisionExists(int id)
        {
            return _context.Decisions.Any(e => e.Id == id);
        }
    }
}
