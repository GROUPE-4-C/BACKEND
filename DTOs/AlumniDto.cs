using AlumniConnect.API.Models;

namespace AlumniConnect.API.DTOs
{
    public class AlumniDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Profession { get; set; }
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Promotion { get; set; }

        public static AlumniDto FromAlumniUser(AlumniUser user)
        {
            return new AlumniDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Profession = user.Profession,
                Bio = user.Bio,
                PhotoUrl = user.PhotoUrl,
                Promotion = user.Promotion?.Nom
            };
        }
    }
}
