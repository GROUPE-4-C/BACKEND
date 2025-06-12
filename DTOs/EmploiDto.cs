// EmploiDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace AlumniConnect.API.DTOs
{
    public class EmploiDto
    {
        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        public string Titre { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(2000, ErrorMessage = "La description ne peut pas dépasser 2000 caractères")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "L'entreprise est obligatoire")]
        [StringLength(200, ErrorMessage = "Le nom de l'entreprise ne peut pas dépasser 200 caractères")]
        public string Entreprise { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La localisation est obligatoire")]
        [StringLength(200, ErrorMessage = "La localisation ne peut pas dépasser 200 caractères")]
        public string Localisation { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le type de contrat est obligatoire")]
        [StringLength(50, ErrorMessage = "Le type de contrat ne peut pas dépasser 50 caractères")]
        public string TypeContrat { get; set; } = string.Empty;
        
        [Range(0, double.MaxValue, ErrorMessage = "Le salaire doit être positif")]
        public decimal? Salaire { get; set; }
        
        [StringLength(10, ErrorMessage = "La devise ne peut pas dépasser 10 caractères")]
        public string? SalaireDevise { get; set; } = "XOF";
        
        [Required(ErrorMessage = "La date de début est obligatoire")]
        public DateTime DateDebut { get; set; }
        
        [Required(ErrorMessage = "La date de fin est obligatoire")]
        public DateTime DateFin { get; set; }
        
        [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
        [StringLength(500, ErrorMessage = "L'URL de l'image ne peut pas dépasser 500 caractères")]
        public string? ImageUrl { get; set; }
        
        public bool EstActif { get; set; } = true;
        
        // Validation personnalisée pour les dates
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateFin <= DateDebut)
            {
                yield return new ValidationResult(
                    "La date de fin doit être postérieure à la date de début",
                    new[] { nameof(DateFin) });
            }
            
            if (DateDebut < DateTime.UtcNow.Date)
            {
                yield return new ValidationResult(
                    "La date de début ne peut pas être dans le passé",
                    new[] { nameof(DateDebut) });
            }
        }
    }
}