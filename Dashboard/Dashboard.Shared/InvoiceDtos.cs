using System.ComponentModel.DataAnnotations;

namespace Dashboard.Shared.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public DateTime DateFacture { get; set; } = DateTime.UtcNow;
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? ClientMatricule { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal TotalTTC { get; set; }
        public decimal TimbreFiscal { get; set; }
        public string Statut { get; set; } = "Brouillon";
        public List<InvoiceLineDto> Lignes { get; set; } = new();
    }

    public class InvoiceLineDto
    {
        public int Id { get; set; }
        public int ProduitId { get; set; }
        public string? ProductName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être au moins 1.")]
        public int Quantite { get; set; }
        public decimal PrixUnitaireHT { get; set; }
        public decimal TauxTVA { get; set; }
        public decimal MontantHT { get; set; }
        public decimal MontantTVA { get; set; }
        public decimal MontantTTC { get; set; }
    }
}
