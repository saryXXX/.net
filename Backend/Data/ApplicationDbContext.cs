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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Soft Delete Global Filter
            modelBuilder.Entity<Client>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Produit>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Facture>().HasQueryFilter(f => !f.IsDeleted);
            modelBuilder.Entity<LigneFacture>().HasQueryFilter(l => !l.IsDeleted);
            modelBuilder.Entity<ParametreFiscal>().HasQueryFilter(pf => !pf.IsDeleted);
        }
    }
}
