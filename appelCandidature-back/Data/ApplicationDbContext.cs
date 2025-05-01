using Microsoft.EntityFrameworkCore;
using pfe_back.Models;

namespace pfe_back.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { 

        }

       
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Candidature> Candidatures { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Decision> Decisions { get; set; }
        public DbSet<Diplome> Diplomes { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<MembreCommission> MembreCommissions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Phase> Phases { get; set; }
        public DbSet<Poste> Postes { get; set; }
        public DbSet<PV> PVs { get; set; }
        public DbSet<Entite> Entites { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TypePoste> TypePostes { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<PieceJointe> PieceJointes { get; set; }
        public DbSet<DecisionPhase> DecisionPhases { get; set; }
        public DbSet<Organigramme> Organigrammes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Commission>()
                .HasMany(c => c.MembreCommissions)
                .WithOne(m => m.Commission)
                .HasForeignKey(f => f.CommissionId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Utilisateurs)
                .WithOne(u => u.Role)
                .HasForeignKey(f => f.RoleId);

            modelBuilder.Entity<Candidat>()
                .HasMany(c => c.Diplomes)
                .WithOne(d => d.Candidat)
                .HasForeignKey(f => f.CandidatId);

            modelBuilder.Entity<Candidat>()
                .HasMany(c => c.Experiences)
                .WithOne(e => e.Candidat)
                .HasForeignKey(f => f.CandidatId);

            modelBuilder.Entity<Candidat>()
                .HasMany(c => c.Candidatures)
                .WithOne(c => c.Candidat)
                .HasForeignKey(f => f.CandidatId);

            modelBuilder.Entity<Candidature>()
                .HasMany(c => c.PieceJointes)
                .WithOne(d => d.Candidature)
                .HasForeignKey(f => f.CandidatureId);

            modelBuilder.Entity<Candidature>()
                .HasMany(c => c.Notifications)
                .WithOne(n => n.Candidature)
                .HasForeignKey(f => f.CandidatureId);

            modelBuilder.Entity<Commission>()
                .HasMany(c => c.PVs)
                .WithOne(p => p.Commission)
                .HasForeignKey(f => f.CommissionId);

            modelBuilder.Entity<Decision>()
                .HasOne(d => d.PV)
                .WithOne(p => p.Decision)
                .HasForeignKey<Decision>(f => f.PVId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Poste>()
                .HasMany(p => p.Candidatures)
                .WithOne(c => c.Poste)
                .HasForeignKey(f => f.PosteId);

            modelBuilder.Entity<Poste>()
                .HasMany(p => p.PVs)
                .WithMany(p => p.Postes)
                .UsingEntity<Dictionary<string, object>>(
                    "PVPoste",
                    j => j.HasOne<PV>().WithMany().HasForeignKey("PVId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Poste>().WithMany().HasForeignKey("PosteId").OnDelete(DeleteBehavior.Restrict)
                );


            modelBuilder.Entity<Poste>()
                .HasMany(p => p.Decisions)
                .WithMany(d => d.Postes)
                .UsingEntity<Dictionary<string, object>>(
                    "DecisionPoste",
                    j => j.HasOne<Decision>().WithMany().HasForeignKey("DecisionId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Poste>().WithMany().HasForeignKey("PosteId").OnDelete(DeleteBehavior.Restrict)
                );

            modelBuilder.Entity<TypePoste>()
                .ToTable("TypePostes");

            modelBuilder.Entity<Entite>()
                .HasMany(p => p.Decisions)
                .WithOne(c => c.Entite)
                .HasForeignKey(f => f.EntiteId);

            modelBuilder.Entity<Entite>()
                .HasMany(p => p.PVs)
                .WithOne(c => c.Entite)
                .HasForeignKey(f => f.EntiteId);


            modelBuilder.Entity<MembreCommission>()
                .HasIndex(mc => new { mc.UtilisateurId, mc.CommissionId })
                .IsUnique();

            modelBuilder.Entity<PieceJointe>()
        .HasOne(pj => pj.PV)
        .WithMany(p => p.PieceJointes)
        .HasForeignKey(pj => pj.PVId)
        .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<PieceJointe>()
        .HasOne(pj => pj.Decision)
        .WithMany(d => d.PieceJointes) 
        .HasForeignKey(pj => pj.DecisionId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Decision>()
                .HasMany(d => d.PieceJointes)
                .WithOne(pj => pj.Decision)
                .HasForeignKey(pj => pj.DecisionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DecisionPhase>()
        .HasKey(dp => new { dp.DecisionId, dp.PhaseId });

            modelBuilder.Entity<DecisionPhase>()
                .HasOne(dp => dp.Decision)
                .WithMany(d => d.DecisionPhases)
                .HasForeignKey(dp => dp.DecisionId);

            modelBuilder.Entity<DecisionPhase>()
                .HasOne(dp => dp.Phase)
                .WithMany(p => p.DecisionPhases)
                .HasForeignKey(dp => dp.PhaseId);

            modelBuilder.Entity<Poste>()
                .HasOne(d => d.Entite)
                .WithMany(p => p.Postes)
                .HasForeignKey(p => p.EntiteId);


            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Nom = "Administrateur" },
                new Role { Id = 2, Nom = "DAO" },
                new Role { Id = 3, Nom = "Candidat" },
                new Role { Id = 4, Nom = "MembreCommission" }
            );



            //modelBuilder.Entity<TypePoste>().HasData(
            //    new TypePoste { Id = 1, Nom = "Service" },
            //    new TypePoste { Id = 2, Nom = "Division" },
            //    new TypePoste { Id = 3, Nom = "Direction" },
            //    new TypePoste { Id = 4, Nom = "Département" },  
            //    new TypePoste { Id = 5, Nom = "Pôle" }

            //    );

        }


        public DbSet<pfe_back.Models.Poste> Poste { get; set; } = default!;
        public DbSet<pfe_back.Models.Decision> Decision { get; set; } = default!;
        public DbSet<pfe_back.Models.PV> PV { get; set; } = default!;
        public DbSet<pfe_back.Models.PostesÀPostuler> PostesÀPostuler { get; set; } = default!;
        public DbSet<pfe_back.Models.Organigramme> Organigramme { get; set; } = default!;
    }
}
