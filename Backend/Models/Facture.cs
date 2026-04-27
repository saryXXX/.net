using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Facture : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Numero { get; set; } = "TEMP-" + Guid.NewGuid().ToString().Substring(0, 8);

        public DateTime DateFacture { get; set; } = DateTime.UtcNow;

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal TotalHT { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal TotalTVA { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal TotalTTC { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal TimbreFiscal { get; set; }

        [StringLength(20)]
        public string Statut { get; set; } = "Brouillon"; // Brouillon, Validée, Payée, Annulée

        public ICollection<LigneFacture> Lignes { get; set; } = new List<LigneFacture>();

        public bool IsDeleted { get; set; } = false;
    }
}
