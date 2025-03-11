using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.DTOs;
using pfe_back.Models;

namespace pfe_back.Controllers
{
    [Authorize(Roles = "DAO")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Commissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commission>>> GetCommissions()
        {
            return await _context.Commissions
                .Include(u => u.MembreCommissions)
                .ToListAsync();
        }

        // GET: api/Commissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Commission>> GetCommission(int id)
        {
            var commission = await _context.Commissions.FindAsync(id);

            if (commission == null)
            {
                return NotFound();
            }

            return commission;
        }

        // PUT: api/Commissions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommission(int id, Commission commission)
        {
            if (id != commission.Id)
            {
                return BadRequest();
            }

            _context.Entry(commission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommissionExists(id))
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

        // POST: api/Commissions
        //[HttpPost]
        //public async Task<IActionResult> PostCommission(Commission commission)
        //{
        //    _context.Commissions.Add(commission);
        //    // Associe les membres existants à la commission sans essayer d'ajouter de nouveaux utilisateurs
        //    var existingMembres = await _context.MembreCommissions
        //        .Where(mc => commission.MembreCommissions.Select(m => m.Id).Contains(mc.Id))
        //        .ToListAsync();

        //    foreach (var membre in commission.MembreCommissions)
        //    {
        //        var existingMembre = existingMembres.FirstOrDefault(mc => mc.Id == membre.Id);
        //        if (existingMembre != null)
        //        {
        //            existingMembre.RoleCommission = membre.RoleCommission;
        //            existingMembre.CommissionId = commission.Id;
        //            _context.Entry(existingMembre).State = EntityState.Modified;
        //        }
        //        else
        //        {
        //            membre.CommissionId = commission.Id;
        //            _context.MembreCommissions.Add(membre);
        //        }
        //    }

        //    // Sauvegarder les modifications dans la base de données
        //    await _context.SaveChangesAsync();

        //    // Retourner la commission mise à jour avec les membres associés
        //    return CreatedAtAction("GetCommission", new { id = commission.Id }, commission);
        //}


        //[HttpPost]
        //public async Task<IActionResult> CreateCommission(Commission commission)
        //{
        //    // Vérifier si le DTO est valide
        //    if (commission == null || commission.MembreCommissions == null || !commission.MembreCommissions.Any())
        //    {
        //        return BadRequest("Données invalides.");
        //    }

        //    // Créer une nouvelle commission
        //    var commiss = new Commission
        //    {
        //        Nom = commission.Nom,
        //        MembreCommissions = new List<MembreCommission>()
        //    };


        //    // Ajouter les membres à la commission
        //    foreach (var membreDto in commission.MembreCommissions)
        //    {
        //        // Vérifier si l'utilisateur existe
        //        var utilisateur = await _context.Utilisateurs.FindAsync(membreDto.UtilisateurId);
        //        if (utilisateur == null)
        //        {
        //            return NotFound($"Utilisateur avec ID {membreDto.Id} non trouvé.");
        //        }


        //        // Vérifier si le membre de commission existe déjà
        //        var membre = _context.MembreCommissions
        //            .FirstOrDefault(m => m.UtilisateurId == utilisateur.Id && m.CommissionId == commission.Id);

        //        if (membre == null)
        //        {
        //            return BadRequest("Non trouvee.");
        //            //// Créer un nouveau membre de commission
        //            //membre = new MembreCommission
        //            //{
        //            //    Nom = utilisateur.Nom, // Optionnel, si vous souhaitez garder les informations de l'utilisateur
        //            //    Prenom = utilisateur.Prenom, // Optionnel
        //            //    Email = utilisateur.Email, // Optionnel
        //            //    RoleCommission = membreDto.RoleCommission,
        //            //    Commission = commission // Lier le membre à la commission
        //            //};
        //            //membre.Role = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "Membre de commission");

        //            //// Ajouter le membre à la collection de membres de la commission
        //            //commission.MembreCommissions.Add(membre);
        //        }
        //        else
        //        {
        //            // Mettre à jour le rôle du membre existant
        //            membre.UtilisateurId = membreDto.UtilisateurId;
        //            membre.CommissionId = membreDto.CommissionId;
        //            membre.RoleCommission = membreDto.RoleCommission;
        //            membre.Commission = membreDto.Commission;
        //            membre.Role = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "Membre de commission");
        //            // Vous pouvez également mettre à jour d'autres propriétés si nécessaire
        //        }
        //    }

        //    // Enregistrer les changements dans la base de données
        //    _context.Commissions.Add(commiss);
        //    await _context.SaveChanges();

        //    return CreatedAtAction(nameof(CreateCommission), new { id = commiss.Id }, commiss);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateCommission(Commission commission)
        //{
        //    // Vérifier si le DTO est valide
        //    if (commission == null)
        //    {
        //        return BadRequest(new { message = "Commission est null." });
        //    }

        //    if (commission.MembreCommissions == null || !commission.MembreCommissions.Any())
        //    {
        //        return BadRequest(new { message = "MembreCommissions est null ou vide." });
        //    }

        //    // Créer une nouvelle commission
        //    var commiss = new Commission
        //    {
        //        Nom = commission.Nom,
        //        MembreCommissions = new List<MembreCommission>()
        //    };

        //    var messages = new List<string>(); // Liste pour stocker les messages

        //    // Ajouter les membres à la commission
        //    foreach (var membreDto in commission.MembreCommissions)
        //    {
        //        messages.Add($"Traitement du membre avec ID: {membreDto.UtilisateurId}");

        //        // Vérifier si l'utilisateur existe
        //        var utilisateur = await _context.Utilisateurs.FindAsync(membreDto.UtilisateurId);
        //        if (utilisateur == null)
        //        {
        //            messages.Add($"Utilisateur avec ID {membreDto.UtilisateurId} non trouvé.");
        //            return NotFound(new { message = messages });
        //        }

        //        messages.Add($"Utilisateur trouvé: {utilisateur.Nom}");

        //        // Vérifier si le membre de commission existe déjà
        //        var membre = await _context.MembreCommissions
        //            .FirstOrDefaultAsync(m => membreDto.UtilisateurId == utilisateur.Id && membreDto.CommissionId == commiss.Id);

        //        if (membre == null)
        //        {
        //            messages.Add("Membre de commission non trouvé, création d'un nouveau membre.");

        //            // Créer un nouveau membre de commission/
        //            membre = new MembreCommission
        //            {
        //                UtilisateurId = utilisateur.Id,
        //                RoleCommission = membreDto.RoleCommission,
        //                Commission = commiss // Lier le membre à la commission
        //            };

        //            // Ajouter le membre à la collection de membres de la commission
        //            commiss.MembreCommissions.Add(membre);
        //        }
        //        else
        //        {
        //            messages.Add("Membre de commission existant trouvé, mise à jour du rôle.");
        //            // Mettre à jour le rôle du membre existant
        //            membre.UtilisateurId = utilisateur.Id;
        //            membre.RoleCommission = membreDto.RoleCommission;
        //            membre.Commission = commiss;
        //            utilisateur.Role = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "Membre de commission");
        //        }
        //    }

        //    // Enregistrer les changements dans la base de données
        //    _context.Commissions.Add(commiss);
        //    await _context.SaveChangesAsync();

        //    messages.Add($"Commission créée avec succès: {commiss.Id}");

        //    return CreatedAtAction(nameof(CreateCommission), new { id = commiss.Id }, new { commission = commiss, messages });
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateCommission(Commission commission)
        //{
        //    // Vérifier si le DTO est valide
        //    if (commission == null)
        //    {
        //        return BadRequest(new { message = "Commission est null." });
        //    }

        //    if (commission.MembreCommissions == null || !commission.MembreCommissions.Any())
        //    {
        //        return BadRequest(new { message = "MembreCommissions est null ou vide." });
        //    }

        //    // Créer une nouvelle commission
        //    var commiss = new Commission
        //    {
        //        Nom = commission.Nom,
        //        MembreCommissions = new List<MembreCommission>()
        //    };

        //    var messages = new List<string>(); // Liste pour stocker les messages

        //    // Ajouter les membres à la commission
        //    foreach (var membreDto in commission.MembreCommissions)
        //    {
        //        messages.Add($"Traitement du membre avec ID: {membreDto.UtilisateurId}");

        //        // Vérifier si l'utilisateur existe
        //        var utilisateur = await _context.Utilisateurs.FindAsync(membreDto.UtilisateurId);
        //        if (utilisateur == null)
        //        {
        //            messages.Add($"Utilisateur avec ID {membreDto.UtilisateurId} non trouvé.");
        //            return NotFound(new { message = messages });
        //        }

        //        messages.Add($"Utilisateur trouvé: {utilisateur.Nom}");


        //        if (membreDto.UtilisateurId == utilisateur.Id && membreDto.CommissionId == commiss.Id)
        //        {
        //            messages.Add("Membre de commission existant trouvé, mise à jour du rôle.");
        //            // Mettre à jour le rôle du membre existant
        //            membreDto.UtilisateurId = utilisateur.Id;
        //            membreDto.RoleCommission = membreDto.RoleCommission;
        //            membreDto.Commission = commiss;
        //            utilisateur.Role = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "Membre de commission");
        //            await _context.SaveChangesAsync();
        //        }
        //    }

        //    // Enregistrer les changements dans la base de données
        //    _context.Commissions.Add(commiss);
        //    await _context.SaveChangesAsync();

        //    messages.Add($"Commission créée avec succès: {commiss.Id}");

        //    return CreatedAtAction(nameof(CreateCommission), new { id = commiss.Id }, new { commission = commiss, messages });
        //}

        [HttpPost]
        public async Task<IActionResult> CreateCommission(Commission commission)
        {
            if (commission == null)
            {
                return BadRequest(new { message = "Commission est null." });
            }

            if (commission.MembreCommissions == null || !commission.MembreCommissions.Any())
            {
                return BadRequest(new { message = "MembreCommissions est null ou vide." });
            }

            // Création de la commission sans l'ID (généré après SaveChangesAsync)
            var commiss = new Commission
            {
                Nom = "Commission temporaire",
                MembreCommissions = new List<MembreCommission>()
            };

            // Récupération des utilisateurs
            var utilisateurIds = commission.MembreCommissions.Select(mc => mc.UtilisateurId).ToList();
            var utilisateurs = await _context.Utilisateurs
                .Where(u => utilisateurIds.Contains(u.Id))
                .ToListAsync();

            // Vérification des membres existants
            //var existingMembres = await _context.MembreCommissions
            //    .Where(mc => utilisateurIds.Contains(mc.UtilisateurId) && mc.CommissionId == commiss.Id)
            //    .ToListAsync();

            // Ajout ou mise à jour des membres
            foreach (var membreDto in commission.MembreCommissions)
            {
                var utilisateur = utilisateurs.FirstOrDefault(u => u.Id == membreDto.UtilisateurId);
                if (utilisateur == null)
                {
                    return NotFound(new { message = $"Utilisateur avec ID {membreDto.UtilisateurId} non trouvé." });
                }

                //var existingMembre = existingMembres.FirstOrDefault(mc => mc.UtilisateurId == membreDto.UtilisateurId);

                //if (existingMembre != null)
                //{
                //    existingMembre.RoleCommission = membreDto.RoleCommission;
                //}
                //else
                //{
                    commiss.MembreCommissions.Add(new MembreCommission
                    {
                        UtilisateurId = utilisateur.Id,
                        RoleCommission = membreDto.RoleCommission,
                        CommissionId = commiss.Id,
                        Utilisateur = utilisateur,
                        Commission = commiss
                    });

                    utilisateur.Role = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "Membre de commission");
                //}
            }

            await _context.Commissions.AddAsync(commiss);
            await _context.SaveChangesAsync(); // L'ID est maintenant généré

            // Mise à jour du nom avec l'ID
            commiss.Nom = $"Commission {commiss.Id}";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Commission créée avec succès.", commissionId = commiss.Id });
        }



        // DELETE: api/Commissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommission(int id)
        {
            var commission = await _context.Commissions.FindAsync(id);
            if (commission == null)
            {
                return NotFound();
            }

            _context.Commissions.Remove(commission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommissionExists(int id)
        {
            return _context.Commissions.Any(e => e.Id == id);
        }
    }
}
