using Microsoft.AspNetCore.Identity;

namespace AlumniConnect.API.Models
{
    public class AlumniUser : IdentityUser
    {
        public string FullName { get; set; }
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public string Profession { get; set; }
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }

        public string? EmailOtp { get; set; }
        public DateTime? EmailOtpExpiration { get; set; }
        public string? ResetPasswordOtp { get; set; }
        public DateTime? ResetPasswordOtpExpiration { get; set; }
    }
}
