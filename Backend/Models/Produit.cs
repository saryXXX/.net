using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Produit : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrixUnitaireHT { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal TauxTVA { get; set; }

        public int StockActuel { get; set; } = 0;

        public int SeuilAlerte { get; set; } = 5;

        public bool Actif { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
    }
}
