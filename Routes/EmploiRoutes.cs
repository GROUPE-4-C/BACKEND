// EmploiRoutes.cs
using AlumniConnect.API.Controllers;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace AlumniConnect.API.Routes
{
    public static class EmploiRoutes
    {
        public static void MapEmploiRoutes(this IEndpointRouteBuilder endpoints)
        {
            // GET: tous les emplois (public)
            endpoints.MapGet("/api/emplois", (EmploiService service) =>
            {
                try
                {
                    var controller = new EmploisController(service);
                    var emplois = controller.GetAll();
                    return Results.Ok(new { success = true, data = emplois, count = emplois.Count() });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la récupération des emplois",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });

            // GET: emplois actifs seulement (public)
            endpoints.MapGet("/api/emplois/actifs", (EmploiService service) =>
            {
                try
                {
                    var controller = new EmploisController(service);
                    var emplois = controller.GetActifs();
                    return Results.Ok(new { success = true, data = emplois, count = emplois.Count() });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la récupération des emplois actifs",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });

            // GET by id: public
            endpoints.MapGet("/api/emplois/{id:guid}", (Guid id, EmploiService service) =>
            {
                try
                {
                    var controller = new EmploisController(service);
                    var emploi = controller.GetById(id);
                    return emploi is null 
                        ? Results.NotFound(new { success = false, message = "Emploi non trouvé" })
                        : Results.Ok(new { success = true, data = emploi });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la récupération de l'emploi",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });

            // GET: emplois par localisation (public)
            endpoints.MapGet("/api/emplois/localisation/{localisation}", (string localisation, EmploiService service) =>
            {
                try
                {
                    var controller = new EmploisController(service);
                    var emplois = controller.GetByLocalisation(localisation);
                    return Results.Ok(new { success = true, data = emplois, count = emplois.Count() });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la recherche par localisation",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });

            // GET: emplois par type de contrat (public)
            endpoints.MapGet("/api/emplois/type/{typeContrat}", (string typeContrat, EmploiService service) =>
            {
                try
                {
                    var controller = new EmploisController(service);
                    var emplois = controller.GetByTypeContrat(typeContrat);
                    return Results.Ok(new { success = true, data = emplois, count = emplois.Count() });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la recherche par type de contrat",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });

            // GET: emplois par utilisateur (public)
            endpoints.MapGet("/api/emplois/user/{userId}", (string userId, EmploiService service) =>
            {
                try
                {
                    var controller = new EmploisController(service);
                    var emplois = controller.GetByUser(userId);
                    return Results.Ok(new { success = true, data = emplois, count = emplois.Count() });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la récupération des emplois de l'utilisateur",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });

            // POST: créer un emploi (protégé)
            endpoints.MapPost("/api/emplois", [Authorize] (EmploiDto dto, EmploiService service, ClaimsPrincipal user) =>
            {
                try
                {
                    // Validation du DTO
                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(dto);
                    if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
                    {
                        var errors = validationResults.Select(vr => new { field = vr.MemberNames.FirstOrDefault(), message = vr.ErrorMessage });
                        return Results.BadRequest(new { success = false, message = "Données invalides", errors = errors });
                    }

                    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null) 
                        return Results.Unauthorized();
                    
                    var controller = new EmploisController(service);
                    var emploiComplet = controller.CreateAndReturn(dto, userId);
                    return Results.Created($"/api/emplois/{emploiComplet.Id}", 
                        new { success = true, message = "Emploi créé avec succès", data = emploiComplet });
                }
                catch (ValidationException ex)
                {
                    return Results.BadRequest(new { success = false, message = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la création de l'emploi",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });

            // PUT: modifier un emploi (protégé)
            endpoints.MapPut("/api/emplois/{id:guid}", [Authorize] (Guid id, EmploiDto dto, EmploiService service, ClaimsPrincipal user) =>
            {
                try
                {
                    // Validation du DTO
                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(dto);
                    if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
                    {
                        var errors = validationResults.Select(vr => new { field = vr.MemberNames.FirstOrDefault(), message = vr.ErrorMessage });
                        return Results.BadRequest(new { success = false, message = "Données invalides", errors = errors });
                    }

                    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null) 
                        return Results.Unauthorized();
                    
                    var controller = new EmploisController(service);
                    var success = controller.Update(id, dto, userId);
                    
                    if (!success)
                        return Results.NotFound(new { success = false, message = "Emploi non trouvé ou accès non autorisé" });
                    
                    return Results.Ok(new { success = true, message = "Emploi modifié avec succès" });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la modification de l'emploi",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });

            // PATCH: désactiver un emploi (protégé)
            endpoints.MapPatch("/api/emplois/{id:guid}/desactiver", [Authorize] (Guid id, EmploiService service, ClaimsPrincipal user) =>
            {
                try
                {
                    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null) 
                        return Results.Unauthorized();
                    
                    var controller = new EmploisController(service);
                    var success = controller.Desactiver(id, userId);
                    
                    if (!success)
                        return Results.NotFound(new { success = false, message = "Emploi non trouvé ou accès non autorisé" });
                    
                    return Results.Ok(new { success = true, message = "Emploi désactivé avec succès" });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la désactivation de l'emploi",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });

            // DELETE: supprimer un emploi (protégé)
            endpoints.MapDelete("/api/emplois/{id:guid}", [Authorize] (Guid id, EmploiService service, ClaimsPrincipal user) =>
            {
                try
                {
                    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null) 
                        return Results.Unauthorized();
                    
                    var controller = new EmploisController(service);
                    var success = controller.Delete(id, userId);
                    
                    if (!success)
                        return Results.NotFound(new { success = false, message = "Emploi non trouvé ou accès non autorisé" });
                    
                    return Results.Ok(new { success = true, message = "Emploi supprimé avec succès" });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: "Erreur lors de la suppression de l'emploi",
                        statusCode: 500,
                        title: "Erreur serveur"
                    );
                }
            });
        }
    }
}