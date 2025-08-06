using System.ComponentModel.DataAnnotations;

namespace AlumniConnect.API.Models
{
    public class Experience
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Poste { get; set; }

        [Required]
        [StringLength(150)]
        public string Entreprise { get; set; }

        [StringLength(100)]
        public string? Localisation { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public bool EnCours { get; set; } = false;

        [StringLength(50)]
        public string? TypeContrat { get; set; } // CDI, CDD, Stage, Freelance, etc.

        [StringLength(100)]
        public string? Secteur { get; set; }

        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        public DateTime? DateModification { get; set; }

        // Relation avec l'utilisateur
        [Required]
        public string UserId { get; set; }

        public AlumniUser User { get; set; }
    }
}
