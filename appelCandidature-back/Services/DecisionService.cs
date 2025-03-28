using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;

namespace pfe_back.Services
{
    public class DecisionService : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DecisionService(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }

        public async Task<ActionResult<Decision>> PostDecision(PV pv)
        {
            if (await _context.Decisions.AnyAsync(d => d.PVId == pv.Id))
            {
                return Conflict("Une décision existe déjà pour ce PV.");
            }

            var maxCandidat = 0;

            if (pv.Postes?.Count > 2)
            {
                maxCandidat = 2;
            } else
            {
                maxCandidat = 1;
            }

            var decision = new Decision
            {
                Reference = pv.Reference,
                EntiteId = pv.EntiteId,
                Statut = "Draft",
                DateCreation = DateTime.UtcNow,
                NbreMaxPosteCandidat = maxCandidat,
                PVId = pv.Id,
                PV = pv,
                Postes = pv.Postes?.ToList() ?? new List<Poste>(),
                PieceJointes = new List<PieceJointe>(),
                DecisionPhases = new List<DecisionPhase>() // Initialisation de la collection
            };

            try
            {
                // Ajouter la décision en base pour générer son ID
                _context.Decisions.Add(decision);
                await _context.SaveChangesAsync(); // On sauvegarde ici pour avoir l'ID de la décision

                // Vérifier si la phase "Draft" existe dans la base
                var existingPhase = await _context.Phases.FirstOrDefaultAsync(p => p.Nom == NomPhase.Draft);

                var draftPhase = new DecisionPhase
                {
                    DateDebut = DateTime.UtcNow,
                    Statut = "Activé",
                    Phase = existingPhase ?? new Phase { Nom = NomPhase.Draft },
                    DecisionId = decision.Id
                };

                decision.DecisionPhases.Add(draftPhase);
                await _context.DecisionPhases.AddAsync(draftPhase);
                Console.WriteLine($"✅ Phase 'Draft' ajoutée");

                await _context.SaveChangesAsync(); // Sauvegarde des modifications

                // Traiter les pièces jointes existantes dans PV
                if (pv.PieceJointes != null)
                {
                    foreach (var pieceJointe in pv.PieceJointes)
                    {
                        var existingPieceJointe = await _context.PieceJointes
                            .FirstOrDefaultAsync(pj => pj.PVId == pv.Id && pj.Nom == pieceJointe.Nom);

                        if (existingPieceJointe != null)
                        {
                            // Mise à jour seulement si nécessaire
                            if (existingPieceJointe.DecisionId != decision.Id || existingPieceJointe.IsFromPV == false)
                            {
                                existingPieceJointe.DecisionId = decision.Id;
                                existingPieceJointe.IsFromPV = true;
                            }
                        }
                        else
                        {
                            var newPieceJointe = new PieceJointe
                            {
                                Nom = pieceJointe.Nom,
                                Fichier = pieceJointe.Fichier,
                                Type = TypePJ.Supp,
                                DecisionId = decision.Id
                            };
                            _context.PieceJointes.Add(newPieceJointe);
                        }
                    }

                    await _context.SaveChangesAsync(); // Sauvegarde des pièces jointes associées
                }

                return Ok(decision);  // Retourner la décision créée
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur lors de la création de la décision : {ex.Message}");
                return BadRequest($"Erreur lors de la création de la décision : {ex.Message}");
            }
        }

    }
}