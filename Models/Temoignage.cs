using System;

namespace AlumniConnect.API.Models
{
    public class Temoignage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public AlumniUser User { get; set; }
        public string Contenu { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
