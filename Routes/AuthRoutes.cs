using AlumniConnect.API.Controllers;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Services;

namespace AlumniConnect.API.Routes
{
    public static class AuthRoutes
    {
        public static void MapAuthRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/api/auth/register", async (RegisterDto dto, AuthService service) =>
            {
                var controller = new AuthController(service);
                return await controller.Register(dto);
            });

            endpoints.MapPost("/api/auth/confirm-email", async (ConfirmEmailDto dto, AuthService service) =>
            {
                var controller = new AuthController(service);
                return await controller.ConfirmEmail(dto);
            });

            endpoints.MapPost("/api/auth/resend-otp", async (ResendOtpDto dto, AuthService service) =>
            {
                var controller = new AuthController(service);
                return await controller.ResendOtp(dto);
            });

            endpoints.MapPost("/api/auth/login", async (LoginDto dto, AuthService service) =>
            {
                var controller = new AuthController(service);
                return await controller.Login(dto);
            });

            endpoints.MapPost("/api/auth/request-reset-password-otp", async (ResendOtpDto dto, AuthService service) =>
            {
                var controller = new AuthController(service);
                return await controller.RequestResetPasswordOtp(dto);
            });

            endpoints.MapPost("/api/auth/reset-password", async (ResetPasswordDto dto, AuthService service) =>
            {
                var controller = new AuthController(service);
                return await controller.ResetPassword(dto);
            });

        }
    }
}
