namespace Dashboard.Shared.DTOs
{
    public class DashboardStatsDto
    {
        public decimal TotalCA { get; set; }
        public decimal TotalTVA { get; set; }
        public int TotalFactures { get; set; }
        public int ProduitsEnAlerte { get; set; }
        public List<ChartDataDto> TurnoverHistory { get; set; } = new();
        public List<TopProductDto> TopProducts { get; set; } = new();
    }

    public class ChartDataDto
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }

    public class TopProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class StockStatusDto
    {
        public string Status { get; set; } = string.Empty; // En Stock, Alerte, Rupture
        public int Count { get; set; }
    }

    public class InventorySummaryDto
    {
        public decimal TotalInventoryValue { get; set; }
        public int TotalProducts { get; set; }
        public int OutOfStockCount { get; set; }
        public int LowStockCount { get; set; }
        public List<StockStatusDto> StatusBreakdown { get; set; } = new();
    }

    public class TvaRateDto
    {
        public decimal Rate { get; set; }
        public decimal TotalTva { get; set; }
    }

    public class FiscalSummaryDto
    {
        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal TotalTTC { get; set; }
        public int FactureCount { get; set; }
        public List<TvaRateDto> TvaDetails { get; set; } = new();
    }
}
