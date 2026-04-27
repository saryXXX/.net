using System.ComponentModel.DataAnnotations;

namespace Dashboard.Shared.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [StringLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Format de téléphone invalide.")]
        public string? Telephone { get; set; }

        public string? Adresse { get; set; }

        [Required(ErrorMessage = "Le Matricule Fiscal est obligatoire.")]
        [RegularExpression(@"^[0-9A-Z]{13}$", ErrorMessage = "Le Matricule Fiscal doit être valide (13 caractères).")]
        public string MatriculeFiscal { get; set; } = string.Empty;

        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    }
}
