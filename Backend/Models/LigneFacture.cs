using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class LigneFacture : IBaseEntity
    {
        public int Id { get; set; }

        public int FactureId { get; set; }
        public Facture? Facture { get; set; }

        public int ProduitId { get; set; }
        public Produit? Produit { get; set; }

        public int Quantite { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal PrixUnitaireHT { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TauxTVA { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal MontantHT { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal MontantTVA { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal MontantTTC { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
