using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Client : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Telephone { get; set; }

        public string? Adresse { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "Le Matricule Fiscal doit contenir exactement 13 chiffres.")]
        public string MatriculeFiscal { get; set; } = string.Empty;

        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
    }
}
