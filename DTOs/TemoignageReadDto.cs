using System;

namespace AlumniConnect.API.DTOs
{
    public class TemoignageReadDto
    {
        public Guid Id { get; set; }
        public string Contenu { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Profession { get; set; }
        public string PhotoUrl { get; set; }
        public string Promotion { get; set; }
    }
}
