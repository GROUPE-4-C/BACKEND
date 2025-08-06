using System.ComponentModel.DataAnnotations;

namespace AlumniConnect.API.DTOs
{
    public class ExperienceDto
    {
        [Required(ErrorMessage = "Le poste est obligatoire")]
        [StringLength(200, ErrorMessage = "Le poste ne peut pas dépasser 200 caractères")]
        public string Poste { get; set; }

        [Required(ErrorMessage = "L'entreprise est obligatoire")]
        [StringLength(150, ErrorMessage = "L'entreprise ne peut pas dépasser 150 caractères")]
        public string Entreprise { get; set; }

        [StringLength(100, ErrorMessage = "La localisation ne peut pas dépasser 100 caractères")]
        public string? Localisation { get; set; }

        [StringLength(2000, ErrorMessage = "La description ne peut pas dépasser 2000 caractères")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "La date de début est obligatoire")]
        public DateTime DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public bool EnCours { get; set; } = false;

        [StringLength(50, ErrorMessage = "Le type de contrat ne peut pas dépasser 50 caractères")]
        public string? TypeContrat { get; set; }

        [StringLength(100, ErrorMessage = "Le secteur ne peut pas dépasser 100 caractères")]
        public string? Secteur { get; set; }
    }
}
