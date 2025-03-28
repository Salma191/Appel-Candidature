using Microsoft.EntityFrameworkCore;
using pfe_back.Data;
using pfe_back.Models;

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
            await _context.SaveChangesAsync(); // Un seul appel pour ajouter et mettre à jour

            // Mise à jour du nom sans re-créer l'entité
            commiss.Nom = $"Commission {commiss.Id}";
            _context.Commissions.Update(commiss); // Assure le suivi de l'entité
            await _context.SaveChangesAsync();

            return commiss.Id;
        }

        public async Task<string?> GetPresidentNameByCommissionId(int commissionId)
        {
            var president = await _context.MembreCommissions
                .Include(mc => mc.Utilisateur) // Inclure les infos de l'utilisateur
                .FirstOrDefaultAsync(mc => mc.CommissionId == commissionId && mc.RoleCommission == RoleCommission.Président);

            return president?.Utilisateur?.Nom; // Récupérer le nom si l'utilisateur existe
        }


    }

}
