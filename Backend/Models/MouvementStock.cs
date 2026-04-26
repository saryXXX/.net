using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class MouvementStock : IBaseEntity
    {
        public int Id { get; set; }

        public int ProduitId { get; set; }
        public Produit? Produit { get; set; }

        [Required]
        [StringLength(20)]
        public string TypeMouvement { get; set; } = "Entrée"; // Entrée, Sortie

        public int Quantite { get; set; }

        public DateTime DateMouvement { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? Reference { get; set; } = string.Empty; // Numero Facture or Order ID

        public string? Observation { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
