using Backend.Common;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public interface IStockService
    {
        Task<Result> AjusterStockAsync(int produitId, int quantite, string type, string reference, string observation = "");
        Task<Result<IEnumerable<MouvementStock>>> GetHistoriqueByProduitAsync(int produitId);
        Task<Result<IEnumerable<Produit>>> GetProduitsEnAlerteAsync();
    }

    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _context;

        public StockService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> AjusterStockAsync(int produitId, int quantite, string type, string reference, string observation = "")
        {
            try
            {
                var produit = await _context.Produits.FindAsync(produitId);
                if (produit == null) return Result.Failure("Product not found.");

                // 1. Create Mouvement
                var mouvement = new MouvementStock
                {
                    ProduitId = produitId,
                    Quantite = quantite,
                    TypeMouvement = type,
                    Reference = reference,
                    Observation = observation,
                    DateMouvement = DateTime.UtcNow
                };

                // 2. Update Produit Stock Level
                if (type == "Entrée")
                {
                    produit.StockActuel += quantite;
                }
                else if (type == "Sortie")
                {
                    if (produit.StockActuel < quantite)
                    {
                        // In some businesses, negative stock is allowed, but we'll flag it or handle it.
                        // For now, we allow it but log it.
                    }
                    produit.StockActuel -= quantite;
                }

                _context.MouvementsStock.Add(mouvement);
                await _context.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<MouvementStock>>> GetHistoriqueByProduitAsync(int produitId)
        {
            var history = await _context.MouvementsStock
                .Where(m => m.ProduitId == produitId)
                .OrderByDescending(m => m.DateMouvement)
                .ToListAsync();
            return Result<IEnumerable<MouvementStock>>.Success(history);
        }

        public async Task<Result<IEnumerable<Produit>>> GetProduitsEnAlerteAsync()
        {
            var alerts = await _context.Produits
                .Where(p => p.StockActuel <= p.SeuilAlerte)
                .ToListAsync();
            return Result<IEnumerable<Produit>>.Success(alerts);
        }
    }
}
