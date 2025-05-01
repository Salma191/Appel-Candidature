using iText.IO.Util;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;
using System.ComponentModel.DataAnnotations;

namespace pfe_back.Services
{
    public class CommissionService
    {
        private readonly ApplicationDbContext _context;

        public CommissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateCommissionAsync(Commission commission)
        {
            if (commission.MembreCommissions == null || !commission.MembreCommissions.Any())
            {
                throw new ArgumentException("La commission doit avoir des membres.");
            }

            var utilisateurIds = commission.MembreCommissions.Select(mc => mc.UtilisateurId).ToList();
            var utilisateurs = await _context.Utilisateurs
                .Where(u => utilisateurIds.Contains(u.Id))
                .ToListAsync();

            var commiss = new Commission
            {
                Nom = "Commission temporaire",
                MembreCommissions = new List<MembreCommission>()
            };

            foreach (var membreDto in commission.MembreCommissions)
            {
                var utilisateur = utilisateurs.FirstOrDefault(u => u.Id == membreDto.UtilisateurId);
                if (utilisateur != null)
                {
                    commiss.MembreCommissions.Add(new MembreCommission
                    {
                        UtilisateurId = utilisateur.Id,
                        RoleCommission = membreDto.RoleCommission,
                        Utilisateur = utilisateur,
                        Commission = commiss
                    });

                    utilisateur.Role = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "MembreCommission");
                }
            }

            await _context.Commissions.AddAsync(commiss);
            await _context.SaveChangesAsync();  

            // Mise à jour du nom sans re-créer l'entité  
            commiss.Nom = $"Commission {commiss.Id}";
            _context.Commissions.Update(commiss);  
            await _context.SaveChangesAsync();

            return commiss.Id;
        }

        public async Task<IEnumerable<object>> GetMembreOfCommission(int commissionId)
        {
            var membres = await _context.MembreCommissions
                .Where(mc => mc.CommissionId == commissionId)
                .Select(mc => new
                {
                    mc.RoleCommission,
                    Utilisateur = _context.Utilisateurs
                        .Where(u => u.Id == mc.UtilisateurId)
                        .Select(u => new { u.Prenom, u.Nom })
                        .FirstOrDefault()
                })
                .ToListAsync();

            var result = membres.Select(res => new
            {
                Prenom = res.Utilisateur?.Prenom ?? "Inconnu",
                Nom = res.Utilisateur?.Nom ?? "Inconnu",
                RoleCommission = EnumUtils.GetEnumDisplayName(res.RoleCommission)
            });

            return result;
        }



    }

}
