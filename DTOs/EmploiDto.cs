using System;

namespace AlumniConnect.API.DTOs
{
    public class EmploiDto
    {
        public string Titre { get; set; }
        public string Description { get; set; }
        public string Entreprise { get; set; }
        public string Localisation { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? ImageUrl { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
