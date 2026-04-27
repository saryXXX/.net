using Backend.Common;
using Backend.Data;
using Dashboard.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public interface IAnalyticsService
    {
        Task<Result<DashboardStatsDto>> GetGeneralStatsAsync();
        Task<Result<List<ChartDataDto>>> GetTurnoverByMonthAsync(int year);
        Task<Result<FiscalSummaryDto>> GetFiscalSummaryAsync(DateTime? startDate, DateTime? endDate);
        Task<Result<InventorySummaryDto>> GetInventorySummaryAsync();
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
                // Only count non-deleted and validated invoices for financial stats
                var validatedFactures = _context.Factures
                    .Where(f => !f.IsDeleted && (f.Statut == "Validée" || f.Statut == "Payée"));

                var stats = new DashboardStatsDto
                {
                    TotalCA = await validatedFactures.SumAsync(f => f.TotalTTC),
                    TotalTVA = await validatedFactures.SumAsync(f => f.TotalTVA),
                    TotalFactures = await validatedFactures.CountAsync(),
                    ProduitsEnAlerte = await _context.Produits.CountAsync(p => !p.IsDeleted && p.StockActuel <= p.SeuilAlerte)
                };

                // Top 5 Products
                stats.TopProducts = await _context.LigneFactures
                    .Include(l => l.Facture)
                    .Where(l => !l.IsDeleted && !l.Facture!.IsDeleted && (l.Facture!.Statut == "Validée" || l.Facture!.Statut == "Payée"))
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
                    .Where(f => !f.IsDeleted && (f.Statut == "Validée" || f.Statut == "Payée") && f.DateFacture.Year == year)
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

        public async Task<Result<FiscalSummaryDto>> GetFiscalSummaryAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var query = _context.Factures
                    .Where(f => !f.IsDeleted && (f.Statut == "Validée" || f.Statut == "Payée"));

                if (startDate.HasValue)
                    query = query.Where(f => f.DateFacture >= startDate.Value);
                
                if (endDate.HasValue)
                    query = query.Where(f => f.DateFacture <= endDate.Value);

                var summary = new FiscalSummaryDto
                {
                    TotalHT = await query.SumAsync(f => f.TotalHT),
                    TotalTVA = await query.SumAsync(f => f.TotalTVA),
                    TotalTTC = await query.SumAsync(f => f.TotalTTC),
                    FactureCount = await query.CountAsync()
                };

                // Detailed TVA by rate (from LigneFactures)
                summary.TvaDetails = await _context.LigneFactures
                    .Include(l => l.Facture)
                    .Where(l => !l.IsDeleted && !l.Facture!.IsDeleted && 
                               (l.Facture.Statut == "Validée" || l.Facture.Statut == "Payée"))
                    .Where(l => (!startDate.HasValue || l.Facture.DateFacture >= startDate.Value) &&
                               (!endDate.HasValue || l.Facture.DateFacture <= endDate.Value))
                    .GroupBy(l => l.TauxTVA)
                    .Select(g => new TvaRateDto
                    {
                        Rate = g.Key,
                        TotalTva = g.Sum(x => x.MontantTVA)
                    })
                    .OrderBy(x => x.Rate)
                    .ToListAsync();

                return Result<FiscalSummaryDto>.Success(summary);
            }
            catch (Exception ex)
            {
                return Result<FiscalSummaryDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<InventorySummaryDto>> GetInventorySummaryAsync()
        {
            try
            {
                var activeProduits = _context.Produits.Where(p => !p.IsDeleted);

                var summary = new InventorySummaryDto
                {
                    TotalProducts = await activeProduits.CountAsync(),
                    OutOfStockCount = await activeProduits.CountAsync(p => p.StockActuel <= 0),
                    LowStockCount = await activeProduits.CountAsync(p => p.StockActuel > 0 && p.StockActuel <= p.SeuilAlerte),
                    TotalInventoryValue = await activeProduits.SumAsync(p => p.StockActuel * p.PrixUnitaireHT)
                };

                summary.StatusBreakdown = new List<StockStatusDto>
                {
                    new StockStatusDto { Status = "Rupture", Count = summary.OutOfStockCount },
                    new StockStatusDto { Status = "Alerte", Count = summary.LowStockCount },
                    new StockStatusDto { Status = "En Stock", Count = summary.TotalProducts - summary.OutOfStockCount - summary.LowStockCount }
                };

                return Result<InventorySummaryDto>.Success(summary);
            }
            catch (Exception ex)
            {
                return Result<InventorySummaryDto>.Failure(ex.Message);
            }
        }
    }
}
