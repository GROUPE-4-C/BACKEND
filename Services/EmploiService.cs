// EmploiService.cs
using System.Collections.Generic;
using System.Linq;
using AlumniConnect.API.Models;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Data;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AlumniConnect.API.Services
{
    public class EmploiService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AlumniUser> _userManager;
        private readonly ILogger<EmploiService> _logger;

        public EmploiService(ApplicationDbContext context, UserManager<AlumniUser> userManager, ILogger<EmploiService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IEnumerable<EmploiReadDto> GetAllEmplois()
        {
            try
            {
                var emplois = _context.Emplois.ToList();
                var userIds = emplois.Select(e => e.UserId).Distinct().ToList();
                var users = _userManager.Users.Where(u => userIds.Contains(u.Id)).ToList();

                return emplois.Select(e =>
                {
                    var user = users.FirstOrDefault(u => u.Id == e.UserId);
                    return new EmploiReadDto
                    {
                        Id = e.Id,
                        Titre = e.Titre ?? string.Empty,
                        Description = e.Description ?? string.Empty,
                        Entreprise = e.Entreprise ?? string.Empty,
                        Localisation = e.Localisation ?? string.Empty,
                        TypeContrat = e.TypeContrat ?? string.Empty,
                        Salaire = e.Salaire,
                        SalaireDevise = e.SalaireDevise ?? "XOF",
                        DateDebut = e.DateDebut,
                        DateFin = e.DateFin,
                        DateCreation = e.DateCreation,
                        ImageUrl = e.ImageUrl,
                        UserId = e.UserId ?? string.Empty,
                        EstActif = e.EstActif,
                        CreatorName = user?.FullName ?? "",
                        CreatorEmail = user?.Email ?? ""
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de tous les emplois");
                throw;
            }
        }

        public IEnumerable<EmploiReadDto> GetEmploisActifs()
        {
            try
            {
                return GetAllEmplois()
                    .Where(e => e.EstActif && !e.EstExpire)
                    .OrderByDescending(e => e.DateCreation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des emplois actifs");
                throw;
            }
        }

        public EmploiReadDto? GetEmploiById(Guid id)
        {
            try
            {
                var e = _context.Emplois.FirstOrDefault(emp => emp.Id == id);
                if (e == null) return null;
                
                var user = _userManager.Users.FirstOrDefault(u => u.Id == e.UserId);
                return new EmploiReadDto
                {
                    Id = e.Id,
                    Titre = e.Titre ?? string.Empty,
                    Description = e.Description ?? string.Empty,
                    Entreprise = e.Entreprise ?? string.Empty,
                    Localisation = e.Localisation ?? string.Empty,
                    TypeContrat = e.TypeContrat ?? string.Empty,
                    Salaire = e.Salaire,
                    SalaireDevise = e.SalaireDevise ?? "XOF",
                    DateDebut = e.DateDebut,
                    DateFin = e.DateFin,
                    DateCreation = e.DateCreation,
                    ImageUrl = e.ImageUrl,
                    UserId = e.UserId ?? string.Empty,
                    EstActif = e.EstActif,
                    CreatorName = user?.FullName ?? "",
                    CreatorEmail = user?.Email ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'emploi {EmploiId}", id);
                throw;
            }
        }

        public Emploi CreateEmploi(EmploiDto dto, string userId)
        {
            try
            {
                // Validation supplémentaire
                if (string.IsNullOrWhiteSpace(userId))
                    throw new ArgumentException("L'ID utilisateur est requis", nameof(userId));

                if (dto.DateFin <= dto.DateDebut)
                    throw new ArgumentException("La date de fin doit être postérieure à la date de début");

                var emploi = new Emploi
                {
                    Id = Guid.NewGuid(),
                    Titre = dto.Titre?.Trim() ?? string.Empty,
                    Description = dto.Description?.Trim() ?? string.Empty,
                    Entreprise = dto.Entreprise?.Trim() ?? string.Empty,
                    Localisation = dto.Localisation?.Trim() ?? string.Empty,
                    TypeContrat = dto.TypeContrat?.Trim() ?? string.Empty,
                    Salaire = dto.Salaire,
                    SalaireDevise = dto.SalaireDevise?.Trim() ?? "XOF",
                    DateDebut = dto.DateDebut,
                    DateFin = dto.DateFin,
                    DateCreation = DateTime.UtcNow,
                    ImageUrl = dto.ImageUrl?.Trim(),
                    UserId = userId,
                    EstActif = dto.EstActif
                };
                
                _context.Emplois.Add(emploi);
                _context.SaveChanges();
                
                _logger.LogInformation("Emploi créé avec succès : {EmploiId} par {UserId}", emploi.Id, userId);
                return emploi;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur de base de données lors de la création de l'emploi pour l'utilisateur {UserId}", userId);
                throw new InvalidOperationException("Erreur lors de la sauvegarde de l'emploi", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'emploi pour l'utilisateur {UserId}", userId);
                throw;
            }
        }

        public EmploiReadDto CreateEmploiAndReturn(EmploiDto dto, string userId)
        {
            try
            {
                var emploi = CreateEmploi(dto, userId);
                var result = GetEmploiById(emploi.Id);
                if (result == null)
                    throw new InvalidOperationException("Impossible de récupérer l'emploi créé");
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création et récupération de l'emploi pour l'utilisateur {UserId}", userId);
                throw;
            }
        }

        public IEnumerable<EmploiReadDto> GetEmploisByUser(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    return Enumerable.Empty<EmploiReadDto>();

                var emplois = _context.Emplois.Where(e => e.UserId == userId).ToList();
                var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

                return emplois.Select(e => new EmploiReadDto
                {
                    Id = e.Id,
                    Titre = e.Titre ?? string.Empty,
                    Description = e.Description ?? string.Empty,
                    Entreprise = e.Entreprise ?? string.Empty,
                    Localisation = e.Localisation ?? string.Empty,
                    TypeContrat = e.TypeContrat ?? string.Empty,
                    Salaire = e.Salaire,
                    SalaireDevise = e.SalaireDevise ?? "XOF",
                    DateDebut = e.DateDebut,
                    DateFin = e.DateFin,
                    DateCreation = e.DateCreation,
                    ImageUrl = e.ImageUrl,
                    UserId = e.UserId ?? string.Empty,
                    EstActif = e.EstActif,
                    CreatorName = user?.FullName ?? "",
                    CreatorEmail = user?.Email ?? ""
                }).OrderByDescending(e => e.DateCreation).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des emplois pour l'utilisateur {UserId}", userId);
                throw;
            }
        }

        public IEnumerable<EmploiReadDto> GetEmploisByLocalisation(string localisation)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(localisation))
                    return Enumerable.Empty<EmploiReadDto>();

                return GetEmploisActifs()
                    .Where(e => e.Localisation.ToLower().Contains(localisation.ToLower().Trim()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche par localisation {Localisation}", localisation);
                throw;
            }
        }

        public IEnumerable<EmploiReadDto> GetEmploisByTypeContrat(string typeContrat)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(typeContrat))
                    return Enumerable.Empty<EmploiReadDto>();

                return GetEmploisActifs()
                    .Where(e => e.TypeContrat.ToLower() == typeContrat.ToLower().Trim());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche par type de contrat {TypeContrat}", typeContrat);
                throw;
            }
        }

        public bool UpdateEmploi(Guid id, EmploiDto dto, string userId)
        {
            try
            {
                var emploi = _context.Emplois.Find(id);
                if (emploi == null || emploi.UserId != userId) 
                    return false;

                // Validation supplémentaire
                if (dto.DateFin <= dto.DateDebut)
                    throw new ArgumentException("La date de fin doit être postérieure à la date de début");
                
                emploi.Titre = dto.Titre?.Trim() ?? string.Empty;
                emploi.Description = dto.Description?.Trim() ?? string.Empty;
                emploi.Entreprise = dto.Entreprise?.Trim() ?? string.Empty;
                emploi.Localisation = dto.Localisation?.Trim() ?? string.Empty;
                emploi.TypeContrat = dto.TypeContrat?.Trim() ?? string.Empty;
                emploi.Salaire = dto.Salaire;
                emploi.SalaireDevise = dto.SalaireDevise?.Trim() ?? "XOF";
                emploi.DateDebut = dto.DateDebut;
                emploi.DateFin = dto.DateFin;
                emploi.ImageUrl = dto.ImageUrl?.Trim();
                emploi.EstActif = dto.EstActif;
                
                _context.SaveChanges();
                
                _logger.LogInformation("Emploi mis à jour avec succès : {EmploiId} par {UserId}", id, userId);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur de base de données lors de la mise à jour de l'emploi {EmploiId} par {UserId}", id, userId);
                throw new InvalidOperationException("Erreur lors de la mise à jour de l'emploi", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de l'emploi {EmploiId} par {UserId}", id, userId);
                throw;
            }
        }

        public bool DeleteEmploi(Guid id, string userId)
        {
            try
            {
                var emploi = _context.Emplois.Find(id);
                if (emploi == null || emploi.UserId != userId) 
                    return false;
                
                _context.Emplois.Remove(emploi);
                _context.SaveChanges();
                
                _logger.LogInformation("Emploi supprimé avec succès : {EmploiId} par {UserId}", id, userId);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur de base de données lors de la suppression de l'emploi {EmploiId} par {UserId}", id, userId);
                throw new InvalidOperationException("Erreur lors de la suppression de l'emploi", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'emploi {EmploiId} par {UserId}", id, userId);
                throw;
            }
        }

        public bool DesactiverEmploi(Guid id, string userId)
        {
            try
            {
                var emploi = _context.Emplois.Find(id);
                if (emploi == null || emploi.UserId != userId) 
                    return false;
                
                emploi.EstActif = false;
                _context.SaveChanges();
                
                _logger.LogInformation("Emploi désactivé avec succès : {EmploiId} par {UserId}", id, userId);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur de base de données lors de la désactivation de l'emploi {EmploiId} par {UserId}", id, userId);
                throw new InvalidOperationException("Erreur lors de la désactivation de l'emploi", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la désactivation de l'emploi {EmploiId} par {UserId}", id, userId);
                throw;
            }
        }
    }
}