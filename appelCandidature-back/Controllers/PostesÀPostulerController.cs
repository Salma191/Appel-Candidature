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
    public class PostesÀPostulerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostesÀPostulerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PostesÀPostuler
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostesÀPostuler>>> GetPostesÀPostuler()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "candidatId");
            if (userIdClaim == null)
                return Unauthorized("Candidat non identifié");

            int candidatId = int.Parse(userIdClaim.Value);
            return await _context.PostesÀPostuler
                .Include(p => p.Poste)
                    .ThenInclude(p => p.TypePoste)
                .Where(p => p.CandidatId == candidatId)
                .ToListAsync();
        }

        // GET: api/PostesÀPostuler/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostesÀPostuler>> GetPostesÀPostuler(int id)
        {
            var postesÀPostuler = await _context.PostesÀPostuler.FindAsync(id);

            if (postesÀPostuler == null)
            {
                return NotFound();
            }

            return postesÀPostuler;
        }

        // PUT: api/PostesÀPostuler/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostesÀPostuler(int id, PostesÀPostuler postesÀPostuler)
        {
            if (id != postesÀPostuler.Id)
            {
                return BadRequest();
            }

            _context.Entry(postesÀPostuler).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostesÀPostulerExists(id))
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

        // POST: api/PostesÀPostuler
        [HttpPost("{posteId}")]
        public async Task<ActionResult<PostesÀPostuler>> PostPostesÀPostuler(int posteId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "candidatId");
            if (userIdClaim == null)
                return Unauthorized("Candidat non identifié");

            int candidatId = int.Parse(userIdClaim.Value);
            var postesÀPostuler = new PostesÀPostuler
            {
                PosteId = posteId,
                Poste = await _context.Poste.FindAsync(candidatId),
                CandidatId = candidatId,
                Candidat = await _context.Candidats.FindAsync(candidatId),
                DateAjout = DateTime.UtcNow
            };
            _context.PostesÀPostuler.Add(postesÀPostuler);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPostesÀPostuler", new { id = postesÀPostuler.Id }, postesÀPostuler);
        }

        // DELETE: api/PostesÀPostuler/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostesÀPostuler(int id)
        {
            var postesÀPostuler = await _context.PostesÀPostuler.FindAsync(id);
            if (postesÀPostuler == null)
            {
                return NotFound();
            }

            _context.PostesÀPostuler.Remove(postesÀPostuler);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostesÀPostulerExists(int id)
        {
            return _context.PostesÀPostuler.Any(e => e.Id == id);
        }
    }
}
