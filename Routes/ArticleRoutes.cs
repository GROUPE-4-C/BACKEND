using AlumniConnect.API.Controllers;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AlumniConnect.API.Routes
{
    public static class ArticleRoutes
    {
        public static void MapArticleRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/api/article", [Authorize] async (CreateArticleDto dto, ArticleService service, ClaimsPrincipal user) =>
            {
                var controller = new ArticleController(service);
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var article = await controller.CreateArticle(dto);
                return Results.Ok(article);
            });

            endpoints.MapGet("/api/article", async (ArticleService service) =>
            {
                var controller = new ArticleController(service);
                var articles = await controller.GetAllArticles();
                return Results.Ok(articles);
            });

            endpoints.MapGet("/api/article/user", [Authorize] async (ArticleService service, ClaimsPrincipal user) =>
            {
                var controller = new ArticleController(service);
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var articles = await controller.GetUserArticles();
                return Results.Ok(articles);
            });

            endpoints.MapPut("/api/article/{id}", [Authorize] async (Guid id, UpdateArticleDto dto, ArticleService service, ClaimsPrincipal user) =>
            {
                var controller = new ArticleController(service);
                var article = await controller.UpdateArticle(id, dto);
                return Results.Ok(article);
            });

            endpoints.MapDelete("/api/article/{id}", [Authorize] async (Guid id, ArticleService service, ClaimsPrincipal user) =>
            {
                var controller = new ArticleController(service);
                await controller.DeleteArticle(id);
                return Results.Ok(new { message = "Article supprimé avec succès" });
            });
        }
    }
} 