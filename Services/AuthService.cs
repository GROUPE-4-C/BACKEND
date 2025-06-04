using AlumniConnect.API.Models;
using AlumniConnect.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace AlumniConnect.API.Services
{
    public class AuthService
    {
        private readonly UserManager<AlumniUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<AlumniUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var user = new AlumniUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                Promotion = dto.Promotion,
                Profession = dto.Profession,
                Bio = "",
                PhotoUrl = ""
            };
            return await _userManager.CreateAsync(user, dto.Password);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AlumniUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp = _config.GetSection("Smtp");
            using var client = new SmtpClient(smtp["Host"], int.Parse(smtp["Port"]))
            {
                Credentials = new NetworkCredential(smtp["User"], smtp["Pass"]),
                EnableSsl = bool.Parse(smtp["EnableSsl"])
            };
            var mail = new MailMessage(smtp["User"], to, subject, body);
            await client.SendMailAsync(mail);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(AlumniUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<AlumniUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(AlumniUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> IsEmailConfirmedAsync(AlumniUser user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task GenerateAndSendOtpAsync(AlumniUser user)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            user.EmailOtp = otp;
            user.EmailOtpExpiration = DateTime.UtcNow.AddMinutes(10);
            await _userManager.UpdateAsync(user);
            await SendEmailAsync(user.Email, "Votre code OTP", $"Votre code de confirmation est : {otp}");
        }

        public async Task<bool> ConfirmEmailWithOtpAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.EmailOtp != otp || user.EmailOtpExpiration < DateTime.UtcNow)
                return false;

            user.EmailConfirmed = true;
            user.EmailOtp = null;
            user.EmailOtpExpiration = null;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ResendOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            await GenerateAndSendOtpAsync(user);
            return true;
        }

        public class ResetPasswordResult
        {
            public bool Success { get; set; }
            public List<string> Errors { get; set; } = new();
        }

        public async Task<ResetPasswordResult> ResetPasswordAsync(string email, string otp, string newPassword)
        {
            var resultObj = new ResetPasswordResult();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.ResetPasswordOtp != otp || user.ResetPasswordOtpExpiration < DateTime.UtcNow)
            {
                resultObj.Errors.Add("OTP invalide ou expiré, ou utilisateur introuvable.");
                return resultObj;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    resultObj.Errors.Add(error.Description);
                }
                return resultObj;
            }

            user.ResetPasswordOtp = null;
            user.ResetPasswordOtpExpiration = null;
            await _userManager.UpdateAsync(user);

            resultObj.Success = true;
            return resultObj;
        }



        public async Task GenerateAndSendResetPasswordOtpAsync(AlumniUser user)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            user.ResetPasswordOtp = otp;
            user.ResetPasswordOtpExpiration = DateTime.UtcNow.AddMinutes(10);
            await _userManager.UpdateAsync(user);
            await SendEmailAsync(user.Email, "OTP de réinitialisation", $"Votre code de réinitialisation est : {otp}");
        }


        public string GenerateJwtToken(AlumniUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FullName ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
