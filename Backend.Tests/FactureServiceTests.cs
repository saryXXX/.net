using Backend.Data;
using Backend.Models;
using Backend.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Backend.Tests
{
    public class FactureServiceTests
    {
        private ApplicationDbContext GetDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateFactureAsync_ShouldCalculateCorrectTotals()
        {
            // Arrange
            var context = GetDatabase();
            var stockMock = new Mock<IStockService>();
            var service = new FactureService(context, stockMock.Object);

            // Set up initial fiscal parameters
            context.ParametresFiscaux.Add(new ParametreFiscal { TimbreFiscal = 1.000m, DateDebut = DateTime.UtcNow.AddDays(-1) });
            await context.SaveChangesAsync();

            var facture = new Facture
            {
                Numero = "FACT-TEST-01",
                Lignes = new List<LigneFacture>
                {
                    new LigneFacture { Quantite = 2, PrixUnitaireHT = 50.000m, TauxTVA = 19.00m }, // HT: 100, TVA: 19, TTC: 119
                    new LigneFacture { Quantite = 1, PrixUnitaireHT = 200.000m, TauxTVA = 10.00m } // HT: 200, TVA: 20, TTC: 220
                }
            };

            // Act
            var result = await service.CreateFactureAsync(facture);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value!.TotalHT.Should().Be(300.000m);
            result.Value.TotalTVA.Should().Be(39.000m);
            result.Value.TotalTTC.Should().Be(340.000m); // 300 + 39 + 1 (Timbre)
            result.Value.TimbreFiscal.Should().Be(1.000m);
        }

        [Fact]
        public async Task ValidateFactureAsync_ShouldTriggerStockSortie()
        {
            // Arrange
            var context = GetDatabase();
            var stockMock = new Mock<IStockService>();
            stockMock.Setup(s => s.AjusterStockAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                     .ReturnsAsync(Backend.Common.Result.Success());

            var service = new FactureService(context, stockMock.Object);

            var facture = new Facture
            {
                Id = 1,
                Numero = "FACT-001",
                Statut = "Brouillon",
                Lignes = new List<LigneFacture>
                {
                    new LigneFacture { ProduitId = 10, Quantite = 5 }
                }
            };
            context.Factures.Add(facture);
            await context.SaveChangesAsync();

            // Act
            var result = await service.ValidateFactureAsync(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            facture.Statut.Should().Be("Validée");
            stockMock.Verify(s => s.AjusterStockAsync(10, 5, "Sortie", "FACT-001", It.IsAny<string>()), Times.Once);
        }
    }
}
