using AlumniConnect.API.DTOs;
using AlumniConnect.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AlumniConnect.API.Routes
{
    public static class ExperienceRoutes
    {
        public static void MapExperienceRoutes(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/experiences").WithTags("Experiences");

            // GET /api/experiences - Récupérer toutes les expériences (public)
            group.MapGet("/", async (ExperienceService service) =>
            {
                try
                {
                    var experiences = await service.GetAllExperiencesAsync();
                    return Results.Ok(experiences);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la récupération des expériences: {ex.Message}");
                }
            })
            .WithName("GetAllExperiences")
            .WithOpenApi();

            // GET /api/experiences/{id} - Récupérer une expérience par son ID (public)
            group.MapGet("/{id:guid}", async (Guid id, ExperienceService service) =>
            {
                try
                {
                    var experience = await service.GetExperienceByIdAsync(id);
                    return experience == null ? Results.NotFound("Expérience non trouvée") : Results.Ok(experience);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la récupération de l'expérience: {ex.Message}");
                }
            })
            .WithName("GetExperienceById")
            .WithOpenApi();

            // GET /api/experiences/user/{userId} - Récupérer les expériences d'un utilisateur (public)
            group.MapGet("/user/{userId}", async (string userId, ExperienceService service) =>
            {
                try
                {
                    var experiences = await service.GetUserExperiencesAsync(userId);
                    return Results.Ok(experiences);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la récupération des expériences de l'utilisateur: {ex.Message}");
                }
            })
            .WithName("GetUserExperiences")
            .WithOpenApi();

            // GET /api/experiences/mes-experiences - Récupérer les expériences de l'utilisateur connecté (authentifié)
            group.MapGet("/mes-experiences", [Authorize] async (ClaimsPrincipal user, ExperienceService service) =>
            {
                try
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Results.Unauthorized();

                    var experiences = await service.GetUserExperiencesAsync(userId);
                    return Results.Ok(experiences);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la récupération de vos expériences: {ex.Message}");
                }
            })
            .WithName("GetMyExperiences")
            .WithOpenApi();

            // GET /api/experiences/search/entreprise/{entreprise} - Rechercher par entreprise (public)
            group.MapGet("/search/entreprise/{entreprise}", async (string entreprise, ExperienceService service) =>
            {
                try
                {
                    var experiences = await service.GetExperiencesByEntrepriseAsync(entreprise);
                    return Results.Ok(experiences);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la recherche par entreprise: {ex.Message}");
                }
            })
            .WithName("GetExperiencesByEntreprise")
            .WithOpenApi();

            // GET /api/experiences/search/secteur/{secteur} - Rechercher par secteur (public)
            group.MapGet("/search/secteur/{secteur}", async (string secteur, ExperienceService service) =>
            {
                try
                {
                    var experiences = await service.GetExperiencesBySecteurAsync(secteur);
                    return Results.Ok(experiences);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la recherche par secteur: {ex.Message}");
                }
            })
            .WithName("GetExperiencesBySecteur")
            .WithOpenApi();

            // GET /api/experiences/current - Récupérer les expériences en cours (public)
            group.MapGet("/current", async (ExperienceService service) =>
            {
                try
                {
                    var experiences = await service.GetCurrentExperiencesAsync();
                    return Results.Ok(experiences);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la récupération des expériences en cours: {ex.Message}");
                }
            })
            .WithName("GetCurrentExperiences")
            .WithOpenApi();

            // POST /api/experiences - Créer une nouvelle expérience (authentifié)
            group.MapPost("/", [Authorize] async (ExperienceDto dto, ClaimsPrincipal user, ExperienceService service) =>
            {
                try
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Results.Unauthorized();

                    var experience = await service.CreateExperienceAsync(dto, userId);
                    return Results.Created($"/api/experiences/{experience.Id}", experience);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la création de l'expérience: {ex.Message}");
                }
            })
            .WithName("CreateExperience")
            .WithOpenApi();

            // PUT /api/experiences/{id} - Modifier une expérience (authentifié, propriétaire uniquement)
            group.MapPut("/{id:guid}", [Authorize] async (Guid id, ExperienceDto dto, ClaimsPrincipal user, ExperienceService service) =>
            {
                try
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Results.Unauthorized();

                    var experience = await service.UpdateExperienceAsync(id, dto, userId);
                    return experience == null ? Results.NotFound("Expérience non trouvée") : Results.Ok(experience);
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Forbid();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la modification de l'expérience: {ex.Message}");
                }
            })
            .WithName("UpdateExperience")
            .WithOpenApi();

            // DELETE /api/experiences/{id} - Supprimer une expérience (authentifié, propriétaire uniquement)
            group.MapDelete("/{id:guid}", [Authorize] async (Guid id, ClaimsPrincipal user, ExperienceService service) =>
            {
                try
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Results.Unauthorized();

                    var success = await service.DeleteExperienceAsync(id, userId);
                    return success ? Results.NoContent() : Results.NotFound("Expérience non trouvée");
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Forbid();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erreur lors de la suppression de l'expérience: {ex.Message}");
                }
            })
            .WithName("DeleteExperience")
            .WithOpenApi();
        }
    }
}
