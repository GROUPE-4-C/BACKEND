using System;

namespace AlumniConnect.API.DTOs
{
    public class EventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
        public string? ImageUrl { get; set; }
    }
}
