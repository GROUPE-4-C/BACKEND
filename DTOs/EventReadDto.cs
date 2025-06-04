using System;

namespace AlumniConnect.API.DTOs
{
    public class EventReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
        public string? ImageUrl { get; set; }
        public string UserId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorEmail { get; set; }
    }
}
