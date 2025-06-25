using AlumniConnect.API.Controllers;
using AlumniConnect.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AlumniConnect.API.Routes
{
    public static class AlumniRoutes
    {
        public static void MapAlumniRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/alumni", (int? promotionId, string? profession, AlumniService service) =>
            {
                var controller = new AlumniController(service);
                return Results.Ok(controller.Search(promotionId, profession));
            });
            endpoints.MapGet("/api/alumni/all", async (AlumniService service) =>
            {
                var alumni = await service.GetAllAlumniExceptAdminsAsync();
                return Results.Ok(alumni);
            });

            endpoints.MapGet("/api/alumni/{id}", async (string id, AlumniService service) =>
            {
                var controller = new AlumniController(service);
                var alumni = await controller.GetById(id);
                if (alumni is null) return Results.NotFound();

                var dto = new AlumniDto
                {
                    Id = alumni.Id,
                    FullName = alumni.FullName,
                    Email = alumni.Email,
                    Profession = alumni.Profession,
                    Bio = alumni.Bio,
                    PhotoUrl = alumni.PhotoUrl,
                    Promotion = alumni.Promotion?.Nom
                };
                return Results.Ok(dto);
            });




        }
    }
}
