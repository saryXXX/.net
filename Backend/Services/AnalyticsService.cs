using Backend.Common;
using Backend.Data;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public interface IAnalyticsService
    {
        Task<Result<DashboardStatsDto>> GetGeneralStatsAsync();
        Task<Result<List<ChartDataDto>>> GetTurnoverByMonthAsync(int year);
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DashboardStatsDto>> GetGeneralStatsAsync()
        {
            try
            {
                // Only count validated invoices for financial stats
                var validatedFactures = _context.Factures
                    .Where(f => f.Statut == "Validée" || f.Statut == "Payée");

                var stats = new DashboardStatsDto
                {
                    TotalCA = await validatedFactures.SumAsync(f => f.TotalTTC),
                    TotalTVA = await validatedFactures.SumAsync(f => f.TotalTVA),
                    TotalFactures = await validatedFactures.CountAsync(),
                    ProduitsEnAlerte = await _context.Produits.CountAsync(p => p.StockActuel <= p.SeuilAlerte)
                };

                // Top 5 Products
                stats.TopProducts = await _context.LigneFactures
                    .Include(l => l.Facture)
                    .Where(l => l.Facture!.Statut == "Validée" || l.Facture!.Statut == "Payée")
                    .GroupBy(l => l.Produit != null ? l.Produit.Nom : "Inconnu")
                    .Select(g => new TopProductDto
                    {
                        ProductName = g.Key,
                        TotalSold = g.Sum(x => x.Quantite),
                        TotalRevenue = g.Sum(x => x.MontantTTC)
                    })
                    .OrderByDescending(x => x.TotalRevenue)
                    .Take(5)
                    .ToListAsync();

                // Simple turnover history (last 6 months)
                var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
                var history = await validatedFactures
                    .Where(f => f.DateFacture >= sixMonthsAgo)
                    .GroupBy(f => new { f.DateFacture.Year, f.DateFacture.Month })
                    .Select(g => new ChartDataDto
                    {
                        Label = $"{g.Key.Month}/{g.Key.Year}",
                        Value = g.Sum(f => f.TotalTTC)
                    })
                    .ToListAsync();
                
                stats.TurnoverHistory = history.OrderBy(h => h.Label).ToList();

                return Result<DashboardStatsDto>.Success(stats);
            }
            catch (Exception ex)
            {
                return Result<DashboardStatsDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<ChartDataDto>>> GetTurnoverByMonthAsync(int year)
        {
            try
            {
                var data = await _context.Factures
                    .Where(f => (f.Statut == "Validée" || f.Statut == "Payée") && f.DateFacture.Year == year)
                    .GroupBy(f => f.DateFacture.Month)
                    .Select(g => new ChartDataDto
                    {
                        Label = g.Key.ToString(),
                        Value = g.Sum(f => f.TotalTTC)
                    })
                    .OrderBy(h => h.Label)
                    .ToListAsync();

                return Result<List<ChartDataDto>>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<List<ChartDataDto>>.Failure(ex.Message);
            }
        }
    }
}
