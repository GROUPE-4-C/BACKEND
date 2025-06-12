// Emploi.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace AlumniConnect.API.Models
{
    public class Emploi
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(200)]
        public string Titre { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Entreprise { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Localisation { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string TypeContrat { get; set; } = string.Empty; // CDI, CDD, Stage, Freelance, etc.
        
        public decimal? Salaire { get; set; }
        
        [MaxLength(10)]
        public string? SalaireDevise { get; set; } = "XOF"; // Devise par défaut
        
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        
        [MaxLength(500)]
        public string? ImageUrl { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        public bool EstActif { get; set; } = true;
    }
}