using AlumniConnect.API.Controllers;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace AlumniConnect.API.Routes
{
    public static class PromotionRoutes
    {
        public static void MapPromotionRoutes(this IEndpointRouteBuilder endpoints)
        {
            // GET all promotions (public)
            endpoints.MapGet("/api/promotions", (PromotionService service) =>
            {
                var controller = new PromotionsController(service);
                return Results.Ok(controller.GetAll());
            });

            // GET by id (public)
            endpoints.MapGet("/api/promotions/{id:int}", (int id, PromotionService service) =>
            {
                var controller = new PromotionsController(service);
                var promo = controller.GetById(id);
                return promo is null ? Results.NotFound() : Results.Ok(promo);
            });

            // PUT update
            endpoints.MapPut("/api/promotions/{id:int}", [Authorize(Roles = "SuperAdmin")] async (int id, PromotionDto dto, PromotionService service) =>
            {
                var controller = new PromotionsController(service);
                var promo = await controller.Update(id, dto);
                return promo is null ? Results.NotFound() : Results.Ok(promo);
            });


            // POST create
            endpoints.MapPost("/api/promotions", [Authorize(Roles = "SuperAdmin")] async (PromotionDto dto, PromotionService service) =>
            {
                var controller = new PromotionsController(service);
                var promo = await controller.Create(dto);
                return Results.Created($"/api/promotions/{promo.Id}", promo);
            });

            // DELETE
            endpoints.MapDelete("/api/promotions/{id:int}", [Authorize(Roles = "SuperAdmin")] async (int id, PromotionService service) =>
            {
                var controller = new PromotionsController(service);
                var ok = await controller.Delete(id);
                return ok ? Results.Ok() : Results.NotFound();
            });
        }
    }
}
