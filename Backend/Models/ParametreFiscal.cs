using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class ParametreFiscal : IBaseEntity
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal TimbreFiscal { get; set; } = 1.000m;

        public DateTime DateDebut { get; set; } = DateTime.UtcNow;

        public DateTime? DateFin { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
