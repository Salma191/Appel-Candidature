using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;
using pfe_back.Services;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf.Event;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Commons.Actions;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.IO.Image;
using pfe_back.DTOs;
using System.Linq;
using NuGet.Versioning;

namespace pfe_back.Controllers
{
    //[Authorize(Roles = "DAO")]
    [Route("api/[controller]")]
    [ApiController]
    public class PVsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CommissionService _commissionService;
        private readonly DecisionService _decisionController;

        public PVsController(ApplicationDbContext context, CommissionService commissionService, DecisionService decisionController)
        {
            _context = context;
            _commissionService = commissionService;
            _decisionController = decisionController;
        }

        // GET: api/PVs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PV>>> GetPV()
        {
            var pvs = await _context.PVs
                .Include(p => p.TypePoste)
                .Include(p => p.Commission)
                .Include(p => p.Postes)
                .Include(P => P.Entite)
                .Include(p => p.PieceJointes)
                .ToListAsync();

            if (pvs == null || pvs.Count == 0)
            {
                return NotFound();
            }

            var result = new List<object>();

            foreach (var pv in pvs)
            {
                var commissionPresident = await _commissionService.GetPresidentNameByCommissionId(pv.CommissionId);
                result.Add(new
                {
                    pv.Id,
                    Entite = pv.Entite?.Nom,
                    pv.DateCreation,
                    pv.Statut,
                    pv.Reference,
                    TypePoste = pv.TypePoste?.Nom,
                    CommissionPresident = commissionPresident,
                    pv.Postes,
                    pv.PieceJointes
                });
            }

            return Ok(result);

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
        public async Task<IActionResult> PutPV(int id, UpdatePVRequest pV)
        {
            var pv = await _context.PVs
                .Include(p => p.TypePoste)
                .Include(p => p.Postes)
                .Include(p => p.PieceJointes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pv == null)
            {
                return NotFound();
            }

            pv.Statut = "Approuvé";
            pv.DateValidation = DateTime.Now;
            pv.PieceJointes ??= new List<PieceJointe>(); // Initialisation si null


            // Vérifier si une nouvelle pièce jointe est fournie
            if (pV.PieceJointe != null)
            {
                var pieceJointe = new PieceJointe
                {
                    Nom = pV.PieceJointe.Nom,
                    Fichier = pV.PieceJointe.Fichier,
                    Type = TypePJ.Signée,
                    PVId = pv.Id
                };
                pv.PieceJointes.Add(pieceJointe);
            }

            // Sauvegarde avant la création de la décision
            await _context.SaveChangesAsync();

            // Vérifier si le PV est bien "Signée" avant de créer la décision
            if (pv.Statut != "Signée")
            {
                throw new Exception("La décision ne peut être créée que si le PV est signé.");
            }

            // Appel à PostDecision
            var decisionResult = await _decisionController.PostDecision(pv);

            if (decisionResult.Result is OkObjectResult okResult && okResult.Value is Decision decision)
            {

            }
            else
            {
                throw new Exception("La décision n'a pas pu être créée.");
            }

            return NoContent();
        }


        // POST: api/PVs
        [HttpPost]
        public async Task<ActionResult<PV>> PostPV(PV pV)
        {
            // Vérification du type de poste
            var typePoste = await _context.TypePostes.FindAsync(pV.TypePosteId);
            if (typePoste == null)
            {
                return BadRequest("Type de poste invalide.");
            }
            else
            {
                Console.WriteLine($"Type de poste trouvé: {typePoste.Nom}");
            }

            // Vérification de l'entité
            var entite = await _context.Entites.FindAsync(pV.EntiteId);
            if (entite == null)
            {
                return BadRequest("Entité invalide.");
            }
            else
            {
                Console.WriteLine($"Entité trouvée: {entite.Nom}");
            }

            // Vérification de la commission
            if (pV.Commission == null)
            {
                return BadRequest("La commission ne peut pas être nulle.");
            }

            if (pV.Commission.MembreCommissions == null)
            {
                return BadRequest("La commission doit avoir des membres.");
            }

            if (pV.Commission.MembreCommissions.Count != 4)
            {
                return BadRequest("La commission doit avoir exactement 4 membres.");
            }

            // Création de la commission et récupération de l'ID
            var commissionId = await _commissionService.CreateCommissionAsync(pV.Commission);
            if (commissionId == 0)
            {
                return BadRequest("Erreur lors de la création de la commission.");
            }
            pV.CommissionId = commissionId;
            Console.WriteLine($"Commission créée avec l'ID: {commissionId}");

            // Suppression de la référence à la commission pour éviter une double création
            pV.Commission = null;

            // Initialisation des autres propriétés du PV
            pV.DateCreation = DateTime.UtcNow;
            pV.Statut = "Draft";

            // Vérification des postes associés
            if (pV.Postes != null)
            {
                var postesExistants = await _context.Postes
                    .Where(p => pV.Postes.Select(poste => poste.Id).Contains(p.Id))
                    .ToListAsync();

                if (postesExistants.Count != pV.Postes.Count)
                {
                    return BadRequest("Un ou plusieurs postes n'ont pas été trouvés dans la base de données.");
                }
                else
                {
                    Console.WriteLine($"Postes trouvés: {postesExistants.Count}");
                }

                foreach (var poste in postesExistants)
                {
                    if (poste.TypePosteId != pV.TypePosteId)
                    {
                        return BadRequest($"Le poste avec l'ID {poste.Id} n'a pas le même TypePosteId que le PV.");
                    }
                }

                // Si tous les postes sont valides, on les associe au PV
                pV.Postes = postesExistants;
            }
            else
            {
                Console.WriteLine("Aucun poste associé au PV.");
            }

            // Ajout du PV à la base de données
            _context.PVs.Add(pV);

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("PV sauvegardé avec succès.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la sauvegarde du PV: {ex.Message}");
            }

            // Retour de la réponse après succès
            return CreatedAtAction("GetPV", new { id = pV.Id }, pV);
        }


        [HttpPost("pj/{pvid}")]
        public async Task<ActionResult<PieceJointe>> AddPJ(int pvid, [FromBody] PieceJointe pieceJointe)
        {
            // Vérifier si le PV existe
            var pv = await _context.PVs.Include(p => p.Decision).FirstOrDefaultAsync(p => p.Id == pvid);
            if (pv == null)
            {
                return NotFound($"Le PV avec l'ID {pvid} n'a pas été trouvé.");
            }

            // Initialiser la liste des pièces jointes si elle est null
            if (pv.PieceJointes == null)
            {
                pv.PieceJointes = new List<PieceJointe>();
            }

            // Créer la nouvelle pièce jointe
            var newPieceJointe = new PieceJointe()
            {
                Nom = pieceJointe.Nom,
                Fichier = pieceJointe.Fichier,
                Type = TypePJ.Supp,
                PVId = pvid
            };

            // Si la décision existe, lier la pièce jointe à la décision
            if (pv.Decision != null)
            {
                newPieceJointe.DecisionId = pv.Decision.Id; // Associer la pièce jointe à la décision
                newPieceJointe.IsFromPV = true;  // Marquer comme venant du PV
            }

            // Ajouter la pièce jointe à la liste des pièces jointes du PV
            pv.PieceJointes.Add(newPieceJointe);
            Console.WriteLine($"Nouvelle PJ ajoutée : Nom = {newPieceJointe.Nom}, PVId = {newPieceJointe.PVId}");

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
                .Where(p => p.PVId == id)
                .ToListAsync();

            var result = new
            {
                Signees = pjs.Where(p => p.Type == TypePJ.Signée).ToList(),
                Supp = pjs.Where(p => p.Type == TypePJ.Supp).ToList()
            };

            return Ok(result); // Retourne un objet contenant deux listes
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

        [HttpGet("generate/{id}")]
        public async Task<IActionResult> GeneratePdf(int id)
        {
            string logoPath = "C:\\Users\\lenovo\\Desktop\\AppelCandidature\\appelCandidature-back\\Assets\\ONEE.png";


            var pv = await _context.PVs
                .Include(p => p.Commission)
                .Include(p => p.Postes)
                .Include(p => p.TypePoste)
                .Include(p => p.Entite)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pv == null)
            {
                return NotFound();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdf);

                // 📌 Ajouter l'event handler pour gérer l'en-tête et le pied de page
                HeaderFooterEventHandler handler = new HeaderFooterEventHandler(logoPath);
                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, handler);


                // ✅ Ajouter le contenu principal du document
                document.Add(new Paragraph("📄 Procès-Verbal de Lancement").SetMarginTop(50));
                document.Add(new Paragraph("Date : " + pv.DateCreation.ToString("dd/MM/yyyy")).SetMarginBottom(10));
                document.Add(new Paragraph("Comité des Nominations : " + await _commissionService.GetPresidentNameByCommissionId(pv.CommissionId)));
                document.Add(new Paragraph("Référence de la Décision : " + pv.Reference));
                document.Add(new Paragraph("Entité du PV : " + pv.Entite?.Nom));


                if (pv.Postes != null && pv.Postes.Count != 0)
                {
                    document.Add(new Paragraph("Liste des Postes :"));
                    foreach (var poste in pv.Postes)
                    {
                        document.Add(new Paragraph($"- {poste.Description}"));
                    }
                }else if (pv.Postes == null || pv.Postes.Count == 0)
                {
                    document.Add(new Paragraph("Aucune poste disponible."));
                }


                document.Close();


                byte[] pdfBytes = ms.ToArray();
                return File(pdfBytes, "application/pdf", "Proces_Verbal_Lancement.pdf");
            }
        }



        private bool PVExists(int id)
        {
            return _context.PVs.Any(e => e.Id == id);
        }
    }
}

//if (User?.Identity?.IsAuthenticated != true)
//{
//    return Unauthorized("Utilisateur non authentifié.");
//}

//    // Récupérer l'email depuis les revendications
// var emailClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

//// Si l'email n'est pas trouvé, retourner une erreur
//if (string.IsNullOrEmpty(emailClaim))
//{
//    return Unauthorized("Email de l'utilisateur introuvable.");
//}

//// Charger l'utilisateur à partir de l'email
//var utilisateurConnecte = await _context.Utilisateurs
//    .Include(u => u.Role)
//    .FirstOrDefaultAsync(u => u.Email == emailClaim);

//if (utilisateurConnecte == null || utilisateurConnecte.Role?.Nom != "DAO")
//{
//    return BadRequest("Seuls les DAO peuvent créer un PV.");
//} else
//{
//    Ok(new { Id = utilisateurConnecte.Id });