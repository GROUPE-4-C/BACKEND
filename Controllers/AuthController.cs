using AlumniConnect.API.DTOs;
using AlumniConnect.API.Services;

namespace AlumniConnect.API.Controllers
{
    public class AuthController
    {
        private readonly AuthService _service;
        public AuthController(AuthService service)
        {
            _service = service;
        }

        public async Task<IResult> Register(RegisterDto dto)
            {
                var result = await _service.RegisterAsync(dto);
                if (!result.Succeeded)
                    return Results.BadRequest(result.Errors);

                return Results.Ok("Inscription réussie. Vérifiez votre email pour le code OTP.");
            }

        public async Task<IResult> ConfirmEmail(ConfirmEmailDto dto)
        {
            var success = await _service.ConfirmEmailWithOtpAsync(dto.Email, dto.Token);
            if (!success) return Results.BadRequest("Code invalide ou expiré.");
            return Results.Ok("Email confirmé !");
        }

        public async Task<IResult> ResendOtp(ResendOtpDto dto)
        {
            var success = await _service.ResendOtpAsync(dto.Email);
            if (!success)
                return Results.BadRequest("Utilisateur introuvable.");
            return Results.Ok("Un nouveau code OTP a été envoyé à votre email.");
        }


        public async Task<IResult> Login(LoginDto dto)
        {
            var user = await _service.FindByEmailAsync(dto.Email);
            if (user == null)
                return Results.BadRequest(new { message = "Email incorrect ou inexistant." });

            var passwordOk = await _service.CheckPasswordAsync(user, dto.Password);
            if (!passwordOk)
                return Results.BadRequest(new { message = "Mot de passe incorrect." });

            if (!await _service.IsEmailConfirmedAsync(user))
                return Results.BadRequest(new { message = "Email non confirmé. Vérifiez votre boîte mail pour le code OTP." });

            var token = _service.GenerateJwtToken(user);

            return Results.Ok(new
            {
                token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    promotion = user.Promotion,
                    profession = user.Profession,
                    bio = user.Bio,
                    photoUrl = user.PhotoUrl,
                    phoneNumber = user.PhoneNumber
                }
            });
        }

        public async Task<IResult> RequestResetPasswordOtp(ResendOtpDto dto)
        {
            var user = await _service.FindByEmailAsync(dto.Email);
            if (user == null)
                return Results.BadRequest("Utilisateur introuvable.");
            await _service.GenerateAndSendResetPasswordOtpAsync(user);
            return Results.Ok("Un code OTP de réinitialisation a été envoyé à votre email.");
        }

        public async Task<IResult> ResetPassword(ResetPasswordDto dto)
        {
            var result = await _service.ResetPasswordAsync(dto.Email, dto.Otp, dto.NewPassword);
            if (!result.Success)
                return Results.BadRequest(new { errors = result.Errors });
            return Results.Ok("Mot de passe réinitialisé avec succès.");
        }




    }
}
