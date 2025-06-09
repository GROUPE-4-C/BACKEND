using AlumniConnect.API.Controllers;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;

namespace AlumniConnect.API.Routes
{
    public static class TemoignageRoutes
    {
        public static void MapTemoignageRoutes(this IEndpointRouteBuilder endpoints)
        {
            // Mur public
            endpoints.MapGet("/api/temoignages", (TemoignageService service) =>
            {
                var controller = new TemoignagesController(service);
                return Results.Ok(controller.GetAll());
            });

            // Liste des témoignages d'un utilisateur
            endpoints.MapGet("/api/temoignages/user/{userId}", (string userId, TemoignageService service) =>
            {
                var controller = new TemoignagesController(service);
                return Results.Ok(controller.GetByUser(userId));
            });

            // Création (authentifié)
            endpoints.MapPost("/api/temoignages", [Authorize] async (TemoignageDto dto, TemoignageService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var controller = new TemoignagesController(service);
                var temoignage = await controller.Create(userId, dto);
                return Results.Created($"/api/temoignages/{temoignage.Id}", temoignage);
            });

            // Modification (admin ou auteur)
            endpoints.MapPut("/api/temoignages/{id:guid}", [Authorize] async (Guid id, TemoignageDto dto, TemoignageService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = user.IsInRole("SuperAdmin");
                var controller = new TemoignagesController(service);
                var updated = await controller.Update(id, userId, isAdmin, dto);
                return updated is null ? Results.Forbid() : Results.Ok(updated);
            });


            // Suppression (admin ou auteur)
            endpoints.MapDelete("/api/temoignages/{id:guid}", [Authorize] async (Guid id, TemoignageService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = user.IsInRole("SuperAdmin");
                var controller = new TemoignagesController(service);
                var ok = await controller.Delete(id, userId, isAdmin);
                return ok ? Results.Ok() : Results.Forbid();
            });
        }
    }
}
