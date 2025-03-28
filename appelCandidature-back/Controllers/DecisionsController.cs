using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;

namespace pfe_back.Controllers
{
    //[Authorize(Roles = "DAO")]
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
            return await _context.Decisions
                .Include(p => p.Entite)
                .Include(p => p.PieceJointes)
                .ToListAsync();
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


        [HttpPost("signe/{decisionid}")]
        public async Task<ActionResult<PieceJointe>> AddPJSignee(int decisionid, [FromBody] PieceJointe pieceJointe)
        {
            // Vérifier si la décision existe
            var decision = await _context.Decisions.FirstOrDefaultAsync(d => d.Id == decisionid);
            if (decision == null)
            {
                return NotFound($"La décision avec l'ID {decisionid} n'a pas été trouvée.");
            }

            // Initialiser la liste des pièces jointes si elle est null
            if (decision.PieceJointes == null)
            {
                decision.PieceJointes = new List<PieceJointe>();
            }

            // Créer la nouvelle pièce jointe
            var newPieceJointe = new PieceJointe()
            {
                Nom = pieceJointe.Nom,
                Fichier = pieceJointe.Fichier,
                Type = TypePJ.Signée,
                DecisionId = decisionid
            };

            // Ajouter la pièce jointe à la liste des pièces jointes du PV
            decision.PieceJointes.Add(newPieceJointe);
            decision.Statut = "Signée";  // Le statut doit être "Signé"
            decision.DateSignature = DateTime.UtcNow;

            try
            {
                // Sauvegarder les modifications dans la base de données
                await _context.SaveChangesAsync();
                return Ok(newPieceJointe);  // Retourner la pièce jointe ajoutée
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de l'ajout de la pièce jointe : {ex.Message}");
            }
        }


        // PUT: api/Decisions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDecision(int id, Decision decision)
        {
            var dec = await _context.Decisions
                .Include(d => d.DecisionPhases)
                .ThenInclude(dp => dp.Phase) // Assurer le chargement des phases
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dec == null)
            {
                Console.WriteLine($"🚨 Décision {id} non trouvée !");
                return NotFound();
            }

            Console.WriteLine($"🔄 Mise à jour de la décision {id} avec statut {dec.Statut}");

            if (dec.Statut == "Signé")
            {
                dec.DatePublication = DateTime.UtcNow;
                dec.DateLimite = dec.DatePublication?.AddDays(21);
                Console.WriteLine($"📅 Date de publication définie : {dec.DatePublication}");

                // Désactivation de la phase "Draft"
                var draftPhase = dec.DecisionPhases.FirstOrDefault(p => p.Phase?.Nom == NomPhase.Draft);
                if (draftPhase != null)
                {
                    draftPhase.Statut = "Désactivé";
                    draftPhase.DateFin = DateTime.UtcNow;
                    _context.Entry(draftPhase).State = EntityState.Modified;
                    Console.WriteLine($"❌ Phase 'Draft' désactivée");
                }

                // Vérifier si la phase "Lancement" existe déjà
                var lancementPhase = dec.DecisionPhases.FirstOrDefault(p => p.Phase?.Nom == NomPhase.Lancement);
                if (lancementPhase == null)
                {
                    // Vérifier si la phase "Lancement" existe dans la base
                    var existingPhase = await _context.Phases.FirstOrDefaultAsync(p => p.Nom == NomPhase.Lancement);

                    lancementPhase = new DecisionPhase
                    {
                        DateDebut = DateTime.UtcNow,
                        Statut = "Activé",
                        Phase = existingPhase ?? new Phase { Nom = NomPhase.Lancement }, // Réutiliser la phase existante si elle existe
                        DecisionId = dec.Id
                    };

                    dec.DecisionPhases.Add(lancementPhase); // Ajouter à la décision
                    await _context.DecisionPhases.AddAsync(lancementPhase);
                    Console.WriteLine($"✅ Phase 'Lancement' ajoutée");
                }

                // Mise à jour du statut de la décision
                dec.Statut = "Lancée";
                _context.Entry(dec).State = EntityState.Modified;

                // Sauvegarde unique des changements
                try
                {
                    Console.WriteLine("💾 Sauvegarde des modifications...");
                    await _context.SaveChangesAsync();
                    Console.WriteLine("✅ Sauvegarde réussie !");
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("⚠️ Erreur de mise à jour concurrentielle");
                    if (!DecisionExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }



        [HttpPost("pj/{decisionid}")]
        public async Task<ActionResult<PieceJointe>> AddPJ(int decisionid, [FromBody] PieceJointe pieceJointe)
        {
            // Vérifier si le PV existe
            var decision = await _context.Decisions.FirstOrDefaultAsync(d => d.Id == decisionid);
            if (decision == null)
            {
                return NotFound($"La decision avec l'ID {decisionid} n'a pas été trouvé.");
            }

            // Initialiser la liste des pièces jointes si elle est null
            if (decision.PieceJointes == null)
            {
                decision.PieceJointes = new List<PieceJointe>();
            }

            // Créer la nouvelle pièce jointe
            var newPieceJointe = new PieceJointe()
            {
                Nom = pieceJointe.Nom,
                Fichier = pieceJointe.Fichier,
                Type = TypePJ.Supp,
                DecisionId = decisionid
            };


            // Ajouter la pièce jointe à la liste des pièces jointes du PV
            decision.PieceJointes.Add(newPieceJointe);

            try
            {
                // Sauvegarder les modifications dans la base de données
                await _context.SaveChangesAsync();
                return Ok(newPieceJointe);  // Retourner la pièce jointe ajoutée
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de l'ajout de la pièce jointe : {ex.Message}");
            }
        }


        [HttpGet("pj/{id}")]
        public async Task<ActionResult<object>> GetPJ(int id)
        {
            var pjs = await _context.PieceJointes
                .Where(p => p.DecisionId == id)
                .ToListAsync();

            var result = new
            {
                Signees = pjs.Where(p => p.Type == TypePJ.Signée).ToList(),
                Supp = pjs.Where(p => p.Type == TypePJ.Supp).ToList()
            };

            return Ok(result); // Retourne un objet contenant deux listes
        }


        [HttpGet("phase/{id}")]
        public async Task<ActionResult<string>> GetPhase(int id)
        {
            var decPhase = await _context.DecisionPhases
                .Include(p => p.Phase)
                .Where(p => p.DecisionId == id && p.Statut == "Activé")
                .FirstOrDefaultAsync();

            var result = decPhase?.Phase?.Nom.ToString();

            return Ok(new { phase = result });
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
