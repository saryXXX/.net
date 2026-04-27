using System.ComponentModel.DataAnnotations;

namespace Dashboard.Shared.DTOs
{
    public class ProduitDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du produit est obligatoire.")]
        [StringLength(200)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0.")]
        public decimal PrixUnitaireHT { get; set; }

        [Required(ErrorMessage = "Le taux de TVA est obligatoire.")]
        public decimal TauxTVA { get; set; }

        public int StockActuel { get; set; } = 0;

        [Required(ErrorMessage = "Le seuil d'alerte est obligatoire.")]
        [Range(0, int.MaxValue, ErrorMessage = "Le seuil d'alerte ne peut pas être négatif.")]
        public int SeuilAlerte { get; set; } = 5;

        public bool Actif { get; set; } = true;
    }
}
