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
    public class DecisionPhasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DecisionPhasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DecisionPhases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DecisionPhase>>> GetDecisionPhases()
        {
            return await _context.DecisionPhases.ToListAsync();
        }

        // GET: api/DecisionPhases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DecisionPhase>> GetDecisionPhase(int id)
        {
            var decisionPhase = await _context.DecisionPhases.FindAsync(id);

            if (decisionPhase == null)
            {
                return NotFound();
            }

            return decisionPhase;
        }

        [HttpGet("parPhase/{phaseId}")]
        public async Task<ActionResult<IEnumerable<Decision>>> GetDecisionByPhase(int phaseId)
        {
            var decPhase = await _context.DecisionPhases
               .Include(p => p.Decision)
                .ThenInclude(d => d.PieceJointes)
               .Include(p => p.Decision)
                .ThenInclude(d => d.Postes)
               .Include(p => p.Decision)
                .ThenInclude(d => d.Entite)
               .Where(p => p.Statut == "Activé" && p.PhaseId == phaseId)
               .ToListAsync();

            return Ok(decPhase);
        }

        [HttpGet("postePublier")]
        public async Task<ActionResult<IEnumerable<Poste>>> GetPostePub()
        {
            var postes = await _context.DecisionPhases
               .Include(p => p.Decision)
                .ThenInclude(d => d.Postes)
               .Where(p => p.Statut == "Activé" && p.Phase.Nom == NomPhase.Preselection)
               .Select((p => new
               {
                   DecisionRef = p.Decision.Reference,
                   Postes = p.Decision.Postes
               }))
               .ToListAsync();

            return Ok(postes);
        }

        // PUT: api/DecisionPhases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDecisionPhase(int id, DecisionPhase decisionPhase)
        {
            if (id != decisionPhase.DecisionId)
            {
                return BadRequest();
            }

            _context.Entry(decisionPhase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DecisionPhaseExists(id))
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

        // POST: api/DecisionPhases
        [HttpPost]
        public async Task<ActionResult<DecisionPhase>> PostDecisionPhase(DecisionPhase decisionPhase)
        {
            _context.DecisionPhases.Add(decisionPhase);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DecisionPhaseExists(decisionPhase.DecisionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDecisionPhase", new { id = decisionPhase.DecisionId }, decisionPhase);
        }

        // DELETE: api/DecisionPhases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDecisionPhase(int id)
        {
            var decisionPhase = await _context.DecisionPhases.FindAsync(id);
            if (decisionPhase == null)
            {
                return NotFound();
            }

            _context.DecisionPhases.Remove(decisionPhase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DecisionPhaseExists(int id)
        {
            return _context.DecisionPhases.Any(e => e.DecisionId == id);
        }
    }
}
