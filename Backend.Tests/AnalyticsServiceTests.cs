using Backend.Data;
using Backend.Models;
using Backend.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests
{
    public class AnalyticsServiceTests
    {
        private ApplicationDbContext GetDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetGeneralStatsAsync_ShouldReturnRealisticCalculations()
        {
            // Arrange
            var context = GetDatabase();
            var service = new AnalyticsService(context);

            var prod = new Produit { Nom = "A", PrixUnitaireHT = 100, TauxTVA = 19, StockActuel = 10 };
            context.Produits.Add(prod);

            var facture = new Facture
            {
                Statut = "Validée",
                DateFacture = DateTime.UtcNow,
                TotalHT = 100,
                TotalTVA = 19,
                TotalTTC = 120, // 100 + 19 + 1 (Timbre)
                TimbreFiscal = 1
            };
            context.Factures.Add(facture);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetGeneralStatsAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value!.TotalCA.Should().Be(120);
            result.Value.TotalTVA.Should().Be(19);
            result.Value.TotalFactures.Should().Be(1);
        }

        [Fact]
        public async Task GetGeneralStatsAsync_ShouldExcludeDeletedRecords()
        {
            // Arrange
            var context = GetDatabase();
            var service = new AnalyticsService(context);

            context.Factures.Add(new Facture
            {
                Statut = "Validée",
                TotalTTC = 500,
                IsDeleted = true // This should be ignored
            });
            
            context.Factures.Add(new Facture
            {
                Statut = "Validée",
                TotalTTC = 200,
                IsDeleted = false // This should be counted
            });

            await context.SaveChangesAsync();

            // Act
            var result = await service.GetGeneralStatsAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value!.TotalCA.Should().Be(200);
            result.Value.TotalFactures.Should().Be(1);
        }
    }
}
