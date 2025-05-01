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
    public class PhasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PhasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Phases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phase>>> GetPhases()
        {
            return await _context.Phases.ToListAsync();
        }

        // GET: api/Phases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Phase>> GetPhase(int id)
        {
            var phase = await _context.Phases.FindAsync(id);

            if (phase == null)
            {
                return NotFound();
            }

            return phase;
        }

        //[HttpGet("start/{phaseId}")]
        //public async Task DemarrerPhaseAsync(int phaseId)
        //{
        //    var phase = await _context.Phases.FirstOrDefaultAsync(p => p.Id == phaseId);
        //    if (phase == null)
        //    {
        //        throw new Exception("Phase non trouvée.");
        //    }

        //    // Lancer la phase
        //    phase.DateDebut = DateTime.Now;
        //    phase.Statut = "En cours";  // Mettre à jour le statut
        //    await _context.SaveChangesAsync();
        //}

        //[HttpGet("end/{phaseId}")]
        //public async Task TerminerPhaseAsync(int phaseId)
        //{
        //    var phase = await _context.Phases.FirstOrDefaultAsync(p => p.Id == phaseId);
        //    if (phase == null)
        //    {
        //        throw new Exception("Phase non trouvée.");
        //    }

        //    // Marquer la phase comme terminée
        //    phase.DateFin = DateTime.Now;
        //    phase.Statut = "Terminé"; // Mettre à jour le statut
        //    await _context.SaveChangesAsync();
        //}


        // PUT: api/Phases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhase(int id, Phase phase)
        {
            if (id != phase.Id)
            {
                return BadRequest();
            }

            _context.Entry(phase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhaseExists(id))
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

        // POST: api/Phases
        [HttpPost]
        public async Task<ActionResult<Phase>> PostPhase(Phase phase)
        {
            _context.Phases.Add(phase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhase", new { id = phase.Id }, phase);
        }

        // DELETE: api/Phases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhase(int id)
        {
            var phase = await _context.Phases.FindAsync(id);
            if (phase == null)
            {
                return NotFound();
            }

            _context.Phases.Remove(phase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhaseExists(int id)
        {
            return _context.Phases.Any(e => e.Id == id);
        }
    }
}
