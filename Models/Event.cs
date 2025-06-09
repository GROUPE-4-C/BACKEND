using System;

namespace AlumniConnect.API.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
        public string? ImageUrl { get; set; }
        public string UserId { get; set; }
    }
}
