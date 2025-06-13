// Emploi.cs - Modèle corrigé
using System;

namespace AlumniConnect.API.Models
{
    public class Emploi
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titre { get; set; }
        public string Description { get; set; }
        public string Entreprise { get; set; }
        public string Localisation { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? ImageUrl { get; set; }
        public string UserId { get; set; } // Ajouté pour lier à un utilisateur
        public bool EstActif { get; set; } = true; // Décommenté
    }
}