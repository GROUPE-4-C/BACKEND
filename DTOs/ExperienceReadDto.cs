using System.ComponentModel.DataAnnotations;

namespace AlumniConnect.API.DTOs
{
    public class ExperienceReadDto
    {
        public Guid Id { get; set; }

        public string Poste { get; set; }

        public string Entreprise { get; set; }

        public string? Localisation { get; set; }

        public string? Description { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public bool EnCours { get; set; }

        public string? TypeContrat { get; set; }

        public string? Secteur { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }

        // Informations sur l'utilisateur
        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public string UserEmail { get; set; }

        public string? UserPhotoUrl { get; set; }

        // Propriété calculée pour la durée
        public string Duree { get; set; }
    }
}
