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
    public static class EventRoutes
    {
        public static void MapEventRoutes(this IEndpointRouteBuilder endpoints)
        {
            // GET: public
            endpoints.MapGet("/api/events", (EventService service) =>
            {
                var controller = new EventsController(service);
                return Results.Ok(controller.GetAll());
            });

            // GET by id: public
            endpoints.MapGet("/api/events/{id:guid}", (Guid id, EventService service) =>
            {
                var controller = new EventsController(service);
                var ev = controller.GetById(id);
                return ev is null ? Results.NotFound() : Results.Ok(ev);
            });

            // POST: protégé
            endpoints.MapPost("/api/events", [Authorize] (EventDto dto, EventService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Results.Unauthorized();
                var controller = new EventsController(service);
                var ev = controller.Create(dto, userId);
                return Results.Created($"/api/events/{ev.Id}", ev);
            });

            // PUT: protégé
            endpoints.MapPut("/api/events/{id:guid}", [Authorize] (Guid id, EventDto dto, EventService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Results.Unauthorized();
                var controller = new EventsController(service);
                return controller.Update(id, dto, userId) ? Results.NoContent() : Results.Forbid();
            });

            // GET: events by userId (public)
            endpoints.MapGet("/api/events/user/{userId}", (string userId, EventService service) =>
            {
                var controller = new EventsController(service);
                return Results.Ok(controller.GetByUser(userId));
            });

            // DELETE: protégé
            endpoints.MapDelete("/api/events/{id:guid}", [Authorize] (Guid id, EventService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Results.Unauthorized();
                var controller = new EventsController(service);
                return controller.Delete(id, userId) ? Results.NoContent() : Results.Forbid();
            });
        }
    }
}
