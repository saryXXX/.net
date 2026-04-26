namespace Backend.DTOs
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

    public class TvaRateDto
    {
        public decimal Rate { get; set; }
        public decimal TotalTva { get; set; }
    }
}
