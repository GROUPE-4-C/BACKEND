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
        }
    }
}
