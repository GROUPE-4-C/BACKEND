namespace AlumniConnect.API.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int PromotionId { get; set; }
        public string Profession { get; set; }
    }
}
