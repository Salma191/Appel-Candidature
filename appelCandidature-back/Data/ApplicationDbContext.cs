using Microsoft.EntityFrameworkCore;
using pfe_back.Models;

namespace pfe_back.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { 

        }

        public DbSet<Administrateur> Administrateurs { get; set; }
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Candidature> Candidatures { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<DAO> DAOs { get; set; }
        public DbSet<Decision> Decisions { get; set; }
        public DbSet<Diplome> Diplomes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<MembreCommission> MembreCommissions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Phase> Phases { get; set; }
        public DbSet<Poste> Postes { get; set; }
        public DbSet<PV> PVs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TypePoste> TypePostes { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Utilisateur>()
                .HasDiscriminator<string>("UtilisateurType")
                .HasValue<DAO>("DAO")
                .HasValue<Candidat>("Candidat")
                .HasValue<MembreCommission>("MembreCommission")
                .HasValue<Administrateur>("Administrateur");

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
                .HasMany(c => c.Documents)
                .WithOne(d => d.Candidature)
                .HasForeignKey(f => f.CandidatureId);

            modelBuilder.Entity<Candidature>()
                .HasMany(c => c.Notifications)
                .WithOne(n => n.Candidature)
                .HasForeignKey(f => f.CandidatureId);

            modelBuilder.Entity<PV>()
                .HasOne(p => p.TypePoste)
                .WithMany(t => t.PVs)
                .HasForeignKey(f => f.TypePosteId);

            modelBuilder.Entity<PV>()
                .HasOne(p => p.DAO)
                .WithMany(d => d.PVs)
                .HasForeignKey(f => f.DAOId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Decision>()
                .HasOne(p => p.DAO)
                .WithMany(d => d.Decisions)
                .HasForeignKey(f => f.DAOId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Commission>()
                .HasMany(c => c.PVs)
                .WithOne(p => p.Commission)
                .HasForeignKey(f => f.CommissionId);

            modelBuilder.Entity<Decision>()
                .HasOne(d => d.PV)
                .WithOne(p => p.Decision)
                .HasForeignKey<Decision>(f => f.PVId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Decision>()
                .HasMany(d => d.Phases)
                .WithOne(p => p.Decision)
                .HasForeignKey(f => f.DecisionId);

            modelBuilder.Entity<Poste>()
                .HasMany(p => p.Candidatures)
                .WithOne(c => c.Poste)
                .HasForeignKey(f => f.PosteId);

            modelBuilder.Entity<Poste>()
                .HasOne(p => p.PV)
                .WithMany(p => p.Postes)
                .HasForeignKey(f => f.PVId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Poste>()
                .HasOne(p => p.Decision)
                .WithMany(d => d.Postes)
                .HasForeignKey(f => f.DecisionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<pfe_back.Models.Poste> Poste { get; set; } = default!;
        public DbSet<pfe_back.Models.Decision> Decision { get; set; } = default!;
        public DbSet<pfe_back.Models.PV> PV { get; set; } = default!;
    }
}
