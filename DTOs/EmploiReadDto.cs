// EmploiReadDto.cs
using System;

namespace AlumniConnect.API.DTOs
{
    public class EmploiReadDto
    {
        public Guid Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string Entreprise { get; set; }
        public string Localisation { get; set; }
        public string TypeContrat { get; set; }
        public decimal? Salaire { get; set; }
        public string? SalaireDevise { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime DateCreation { get; set; }
        public string? ImageUrl { get; set; }
        public string UserId { get; set; }
        public bool EstActif { get; set; }
        public string CreatorName { get; set; }
        public string CreatorEmail { get; set; }
        
        // Propriétés calculées utiles
        public bool EstExpire => DateTime.UtcNow > DateFin;
        public int JoursRestants => EstExpire ? 0 : (int)(DateFin - DateTime.UtcNow).TotalDays;
    }
}