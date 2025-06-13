// EmploiReadDto.cs - DTO de lecture corrigé
using System;

namespace AlumniConnect.API.DTOs
{
    public class EmploiReadDto
    {
        public Guid Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Entreprise { get; set; } = string.Empty;
        public string Localisation { get; set; } = string.Empty;
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? ImageUrl { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;
        public string CreatorEmail { get; set; } = string.Empty;
        public bool EstActif { get; set; }
    }
}