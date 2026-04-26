using Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Produit> Produits => Set<Produit>();
        public DbSet<Facture> Factures => Set<Facture>();
        public DbSet<LigneFacture> LigneFactures => Set<LigneFacture>();
        public DbSet<ParametreFiscal> ParametresFiscaux => Set<ParametreFiscal>();
        public DbSet<MouvementStock> MouvementsStock => Set<MouvementStock>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Soft Delete Global Filter
            modelBuilder.Entity<Client>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Produit>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Facture>().HasQueryFilter(f => !f.IsDeleted);
            modelBuilder.Entity<LigneFacture>().HasQueryFilter(l => !l.IsDeleted);
            modelBuilder.Entity<ParametreFiscal>().HasQueryFilter(pf => !pf.IsDeleted);
            modelBuilder.Entity<MouvementStock>().HasQueryFilter(m => !m.IsDeleted);

            // Optimization Indexes
            modelBuilder.Entity<Facture>().HasIndex(f => f.DateFacture);
            modelBuilder.Entity<Facture>().HasIndex(f => f.ClientId);
            modelBuilder.Entity<Facture>().HasIndex(f => f.Numero).IsUnique();
            modelBuilder.Entity<MouvementStock>().HasIndex(m => m.ProduitId);
            modelBuilder.Entity<Produit>().HasIndex(p => p.Nom);
        }
    }
}
