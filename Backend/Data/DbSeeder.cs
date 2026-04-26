using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // 1. Fiscal Parameters
            if (!await context.ParametresFiscaux.AnyAsync())
            {
                context.ParametresFiscaux.Add(new ParametreFiscal
                {
                    TimbreFiscal = 1.000m,
                    DateDebut = DateTime.UtcNow.AddYears(-1)
                });
            }

            // 2. Clients
            if (!await context.Clients.AnyAsync())
            {
                context.Clients.AddRange(
                    new Client { Nom = "Client Alpha", Prenom = "Jean", Email = "jean@alpha.com", MatriculeFiscal = "1234567890123", Telephone = "21612345678" },
                    new Client { Nom = "Client Beta", Prenom = "Marie", Email = "marie@beta.com", MatriculeFiscal = "9876543210987", Telephone = "21687654321" }
                );
            }

            // 3. Products
            if (!await context.Produits.AnyAsync())
            {
                context.Produits.AddRange(
                    new Produit { Nom = "Laptop Pro", PrixUnitaireHT = 2500.000m, TauxTVA = 19.00m, StockActuel = 10, SeuilAlerte = 3 },
                    new Produit { Nom = "Mouse Wireless", PrixUnitaireHT = 45.000m, TauxTVA = 19.00m, StockActuel = 2, SeuilAlerte = 5 }, // Alert
                    new Produit { Nom = "Monitor 4K", PrixUnitaireHT = 850.000m, TauxTVA = 19.00m, StockActuel = 15, SeuilAlerte = 5 }
                );
            }

            await context.SaveChangesAsync();

            // 4. Invoices (Only if none exist)
            if (!await context.Factures.AnyAsync())
            {
                var client = await context.Clients.FirstAsync();
                var p1 = await context.Produits.FirstAsync(p => p.Nom == "Laptop Pro");
                var p2 = await context.Produits.FirstAsync(p => p.Nom == "Mouse Wireless");

                var facture = new Facture
                {
                    Numero = "FACT-2026-0001",
                    ClientId = client.Id,
                    DateFacture = DateTime.UtcNow.AddDays(-2),
                    Statut = "Validée",
                    TimbreFiscal = 1.000m,
                    Lignes = new List<LigneFacture>
                    {
                        new LigneFacture 
                        { 
                            ProduitId = p1.Id, 
                            Quantite = 1, 
                            PrixUnitaireHT = p1.PrixUnitaireHT, 
                            TauxTVA = p1.TauxTVA,
                            MontantHT = p1.PrixUnitaireHT,
                            MontantTVA = p1.PrixUnitaireHT * (p1.TauxTVA / 100),
                            MontantTTC = p1.PrixUnitaireHT * (1 + p1.TauxTVA / 100)
                        },
                        new LigneFacture 
                        { 
                            ProduitId = p2.Id, 
                            Quantite = 2, 
                            PrixUnitaireHT = p2.PrixUnitaireHT, 
                            TauxTVA = p2.TauxTVA,
                            MontantHT = p2.PrixUnitaireHT * 2,
                            MontantTVA = (p2.PrixUnitaireHT * 2) * (p2.TauxTVA / 100),
                            MontantTTC = (p2.PrixUnitaireHT * 2) * (1 + p2.TauxTVA / 100)
                        }
                    }
                };

                facture.TotalHT = facture.Lignes.Sum(l => l.MontantHT);
                facture.TotalTVA = facture.Lignes.Sum(l => l.MontantTVA);
                facture.TotalTTC = facture.TotalHT + facture.TotalTVA + facture.TimbreFiscal;

                context.Factures.Add(facture);

                // 5. Initial Stock Movements
                context.MouvementsStock.AddRange(
                    new MouvementStock { ProduitId = p1.Id, Quantite = 11, TypeMouvement = "Entrée", Reference = "SEED", Observation = "Initial Seed" },
                    new MouvementStock { ProduitId = p2.Id, Quantite = 4, TypeMouvement = "Entrée", Reference = "SEED", Observation = "Initial Seed" },
                    // Sortie for the validated invoice
                    new MouvementStock { ProduitId = p1.Id, Quantite = 1, TypeMouvement = "Sortie", Reference = facture.Numero, Observation = "Vente" },
                    new MouvementStock { ProduitId = p2.Id, Quantite = 2, TypeMouvement = "Sortie", Reference = facture.Numero, Observation = "Vente" }
                );

                // Correct actual stock levels (Manual seed adjustment)
                p1.StockActuel = 10;
                p2.StockActuel = 2;

                await context.SaveChangesAsync();
            }
        }
    }
}
