using AlumniConnect.API.Controllers;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;

namespace AlumniConnect.API.Routes
{
    public static class EmploiRoutes
    {
        public static void MapEmploiRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/emplois", (EmploiService service) =>
            {
                var controller = new EmploisController(service);
                return Results.Ok(controller.GetAll());
            });

            // GET: public - emplois actifs uniquement
            endpoints.MapGet("/api/emplois/actifs", (EmploiService service) =>
            {
                var controller = new EmploisController(service);
                return Results.Ok(controller.GetActifs());
            });

            // GET by id: public
            endpoints.MapGet("/api/emplois/{id:guid}", (Guid id, EmploiService service) =>
            {
                var controller = new EmploisController(service);
                var emploi = controller.GetById(id);
                return emploi is null ? Results.NotFound() : Results.Ok(emploi);
            });

            // POST: protégé
            endpoints.MapPost("/api/emplois", [Authorize] (EmploiDto dto, EmploiService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Results.Unauthorized();
                var controller = new EmploisController(service);
                var emploi = controller.Create(dto, userId);
                return Results.Created($"/api/emplois/{emploi.Id}", emploi);
            });

            // PUT: protégé - Return updated EmploiReadDto
            endpoints.MapPut("/api/emplois/{id:guid}", [Authorize] (Guid id, EmploiDto dto, EmploiService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Results.Unauthorized();

                var controller = new EmploisController(service);
                if (!controller.Update(id, dto, userId))
                    return Results.Forbid();

                // Fetch the updated emploi to return as JSON
                var updatedEmploi = controller.GetById(id);
                return updatedEmploi != null ? Results.Ok(updatedEmploi) : Results.NotFound();
            });

            // GET: emplois by userId (public)
            endpoints.MapGet("/api/emplois/user/{userId}", (string userId, EmploiService service) =>
            {
                var controller = new EmploisController(service);
                return Results.Ok(controller.GetByUser(userId));
            });

            // DELETE: protégé - Return confirmation message
            endpoints.MapDelete("/api/emplois/{id:guid}", [Authorize] (Guid id, EmploiService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Results.Unauthorized();
                var controller = new EmploisController(service);
                if (!controller.Delete(id, userId))
                    return Results.Forbid();

                // Return a JSON confirmation
                return Results.Ok(new { Message = $"Emploi with ID {id} successfully deleted." });
            });
        }
    }
}
