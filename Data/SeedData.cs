using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AlumniConnect.API.Models;

namespace AlumniConnect.API.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AlumniUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed roles
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

            // Seed promotions
            if (!context.Promotions.Any())
            {
                context.Promotions.AddRange(
                    new Promotion { Nom = "Promo 2020" },
                    new Promotion { Nom = "Promo 2021" },
                    new Promotion { Nom = "Promo 2022" }
                );
                await context.SaveChangesAsync();
            }

            // Seed super admin
            var adminEmail = "admin@alumni.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            var promo = context.Promotions.First();
            if (admin == null)
            {
                admin = new AlumniUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Super Admin",
                    PromotionId = promo.Id,
                    Profession = "Admin",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "SuperAdmin");
            }
            else
            {
                if (!admin.EmailConfirmed)
                {
                    admin.EmailConfirmed = true;
                    await userManager.UpdateAsync(admin);
                }
            }


        }
    }
}
