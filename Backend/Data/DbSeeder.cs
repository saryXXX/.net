using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            // 0. Default Admin User
            if (!await userManager.Users.AnyAsync())
            {
                var admin = new IdentityUser { UserName = "admin@sary.tn", Email = "admin@sary.tn", EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin123!");
            }

            // 1. Fiscal Parameters
            if (!await context.ParametresFiscaux.AnyAsync())
            {
                context.ParametresFiscaux.Add(new ParametreFiscal { TimbreFiscal = 1.000m, DateDebut = DateTime.UtcNow.AddYears(-2) });
            }

            // 2. Clients (Scale up to 15)
            if (await context.Clients.CountAsync() < 10)
            {
                var clients = new List<Client>
                {
                    new Client { Nom = "Tech Solutions", Prenom = "Ahmed", Email = "contact@techsol.tn", MatriculeFiscal = "1234567MAM000", Telephone = "71234567", Adresse = "Tunis, Berges du Lac" },
                    new Client { Nom = "Global Export", Prenom = "Sami", Email = "sami@global.com", MatriculeFiscal = "9876543BAB000", Telephone = "72345678", Adresse = "Sousse, Zone Industrielle" },
                    new Client { Nom = "Design Studio", Prenom = "Lina", Email = "lina@design.tn", MatriculeFiscal = "4561237CAC000", Telephone = "73456789", Adresse = "Sfax, Route de Gremda" },
                    new Client { Nom = "Agro Food", Prenom = "Omar", Email = "admin@agrofood.tn", MatriculeFiscal = "7894561DAD000", Telephone = "74567890", Adresse = "Bizerte, Port" },
                    new Client { Nom = "Individual Buyer", Prenom = "Mounir", Email = "mounir@gmail.com", MatriculeFiscal = "0001112EAE000", Telephone = "22334455", Adresse = "Nabeul" },
                    new Client { Nom = "Elite Consulting", Prenom = "Yassine", Email = "yassine@elite.com", MatriculeFiscal = "1112223FAF000", Telephone = "75123456", Adresse = "Tunis, Ennasr" },
                    new Client { Nom = "Pharma Plus", Prenom = "Amira", Email = "info@pharma.tn", MatriculeFiscal = "4445556GAG000", Telephone = "76234567", Adresse = "Monastir" },
                    new Client { Nom = "Logistics TN", Prenom = "Karim", Email = "karim@logistics.tn", MatriculeFiscal = "7778889HAH000", Telephone = "77345678", Adresse = "Rades" },
                    new Client { Nom = "Smart Dev", Prenom = "Ines", Email = "ines@smartdev.tn", MatriculeFiscal = "0009998IAI000", Telephone = "78456789", Adresse = "Ariana" },
                    new Client { Nom = "Construct Plus", Prenom = "Tarak", Email = "tarak@construct.com", MatriculeFiscal = "2223334JAJ000", Telephone = "79567890", Adresse = "Kairouan" }
                };
                context.Clients.AddRange(clients);
                await context.SaveChangesAsync();
            }

            // 3. Products (Scale up to 20)
            if (await context.Produits.CountAsync() < 15)
            {
                var products = new List<Produit>
                {
                    new Produit { Nom = "Laptop Dell XPS 15", PrixUnitaireHT = 4200.000m, TauxTVA = 19.00m, StockActuel = 8, SeuilAlerte = 2 },
                    new Produit { Nom = "MacBook Air M2", PrixUnitaireHT = 3800.000m, TauxTVA = 19.00m, StockActuel = 5, SeuilAlerte = 2 },
                    new Produit { Nom = "Monitor Dell 27\"", PrixUnitaireHT = 950.000m, TauxTVA = 19.00m, StockActuel = 12, SeuilAlerte = 5 },
                    new Produit { Nom = "Keyboard Mechanical", PrixUnitaireHT = 180.000m, TauxTVA = 19.00m, StockActuel = 25, SeuilAlerte = 10 },
                    new Produit { Nom = "Docking Station USB-C", PrixUnitaireHT = 450.000m, TauxTVA = 19.00m, StockActuel = 3, SeuilAlerte = 5 },
                    new Produit { Nom = "External SSD 1TB", PrixUnitaireHT = 320.000m, TauxTVA = 19.00m, StockActuel = 20, SeuilAlerte = 8 },
                    new Produit { Nom = "Webcam 4K", PrixUnitaireHT = 280.000m, TauxTVA = 19.00m, StockActuel = 1, SeuilAlerte = 3 },
                    new Produit { Nom = "Office Chair Ergonomic", PrixUnitaireHT = 750.000m, TauxTVA = 19.00m, StockActuel = 10, SeuilAlerte = 3 },
                    new Produit { Nom = "Printer LaserJet", PrixUnitaireHT = 1200.000m, TauxTVA = 7.00m, StockActuel = 4, SeuilAlerte = 2 },
                    new Produit { Nom = "Toner Cartridge Black", PrixUnitaireHT = 150.000m, TauxTVA = 19.00m, StockActuel = 50, SeuilAlerte = 10 },
                    new Produit { Nom = "Smartphone Pro Max", PrixUnitaireHT = 3200.000m, TauxTVA = 19.00m, StockActuel = 15, SeuilAlerte = 3 },
                    new Produit { Nom = "Tablet Pro 12.9", PrixUnitaireHT = 2900.000m, TauxTVA = 19.00m, StockActuel = 7, SeuilAlerte = 2 },
                    new Produit { Nom = "Wireless Earbuds", PrixUnitaireHT = 350.000m, TauxTVA = 19.00m, StockActuel = 40, SeuilAlerte = 5 },
                    new Produit { Nom = "Network Router WiFi 6", PrixUnitaireHT = 520.000m, TauxTVA = 19.00m, StockActuel = 12, SeuilAlerte = 4 },
                    new Produit { Nom = "Projector 4K Office", PrixUnitaireHT = 1800.000m, TauxTVA = 19.00m, StockActuel = 3, SeuilAlerte = 1 },
                    new Produit { Nom = "UPS 1500VA", PrixUnitaireHT = 650.000m, TauxTVA = 19.00m, StockActuel = 8, SeuilAlerte = 2 },
                    new Produit { Nom = "Graphic Tablet L", PrixUnitaireHT = 480.000m, TauxTVA = 19.00m, StockActuel = 10, SeuilAlerte = 3 },
                    new Produit { Nom = "Backpack Tech Pro", PrixUnitaireHT = 120.000m, TauxTVA = 19.00m, StockActuel = 30, SeuilAlerte = 5 },
                    new Produit { Nom = "USB Hub 7-Port", PrixUnitaireHT = 85.000m, TauxTVA = 19.00m, StockActuel = 25, SeuilAlerte = 5 },
                    new Produit { Nom = "Desk Lamp LED", PrixUnitaireHT = 75.000m, TauxTVA = 19.00m, StockActuel = 18, SeuilAlerte = 4 }
                };
                context.Produits.AddRange(products);
                await context.SaveChangesAsync();
            }

            // 4. Heavy Invoicing History (80 Invoices over 12 months)
            if (await context.Factures.CountAsync() < 50)
            {
                var random = new Random();
                var clients = await context.Clients.ToListAsync();
                var products = await context.Produits.ToListAsync();
                var timbres = await context.ParametresFiscaux.OrderByDescending(p => p.DateDebut).FirstOrDefaultAsync();
                var timbreValue = timbres?.TimbreFiscal ?? 1.000m;

                for (int i = 1; i <= 80; i++)
                {
                    var date = DateTime.UtcNow.AddMonths(-random.Next(0, 13)).AddDays(-random.Next(1, 28));
                    var client = clients[random.Next(clients.Count)];
                    
                    var facture = new Facture
                    {
                        Numero = $"FACT-{date.Year}-{date.Month:D2}-{i:D4}",
                        ClientId = client.Id,
                        DateFacture = date,
                        Statut = (random.Next(1, 10) > 8) ? "Brouillon" : (random.Next(1, 10) > 5) ? "Validée" : "Payée",
                        TimbreFiscal = timbreValue,
                        Lignes = new List<LigneFacture>()
                    };

                    var numLines = random.Next(1, 6);
                    for (int j = 0; j < numLines; j++)
                    {
                        var prod = products[random.Next(products.Count)];
                        var qty = random.Next(1, 5);
                        
                        facture.Lignes.Add(new LigneFacture
                        {
                            ProduitId = prod.Id,
                            Quantite = qty,
                            PrixUnitaireHT = prod.PrixUnitaireHT,
                            TauxTVA = prod.TauxTVA,
                            MontantHT = qty * prod.PrixUnitaireHT,
                            MontantTVA = (qty * prod.PrixUnitaireHT) * (prod.TauxTVA / 100),
                            MontantTTC = (qty * prod.PrixUnitaireHT) * (1 + prod.TauxTVA / 100)
                        });
                    }

                    facture.TotalHT = facture.Lignes.Sum(l => l.MontantHT);
                    facture.TotalTVA = facture.Lignes.Sum(l => l.MontantTVA);
                    facture.TotalTTC = facture.TotalHT + facture.TotalTVA + facture.TimbreFiscal;

                    context.Factures.Add(facture);

                    if (facture.Statut != "Brouillon")
                    {
                        foreach (var l in facture.Lignes)
                        {
                            context.MouvementsStock.Add(new MouvementStock
                            {
                                ProduitId = l.ProduitId,
                                Quantite = l.Quantite,
                                TypeMouvement = "Sortie",
                                DateMouvement = date,
                                Reference = facture.Numero,
                                Observation = "Sales Sync"
                            });
                        }
                    }
                }

                // Boost initial stock for all products to prevent negative levels during demo
                foreach (var p in products)
                {
                    context.MouvementsStock.Add(new MouvementStock
                    {
                        ProduitId = p.Id,
                        Quantite = 500,
                        TypeMouvement = "Entrée",
                        DateMouvement = DateTime.UtcNow.AddYears(-1).AddMonths(-1),
                        Reference = "RESTOCK-INIT",
                        Observation = "Bulk Initialization"
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
