
// EmploiService.cs - Service corrigé
using System.Collections.Generic;
using System.Linq;
using AlumniConnect.API.Models;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Data;
using System;
using Microsoft.AspNetCore.Identity;

namespace AlumniConnect.API.Services
{
    public class EmploiService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AlumniUser> _userManager;

        public EmploiService(ApplicationDbContext context, UserManager<AlumniUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<EmploiReadDto> GetAllEmplois()
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
                    Titre = e.Titre,
                    Description = e.Description,
                    Entreprise = e.Entreprise,
                    Localisation = e.Localisation,
                    DateDebut = e.DateDebut,
                    DateFin = e.DateFin,
                    ImageUrl = e.ImageUrl,
                    UserId = e.UserId,
                    CreatorName = user?.FullName ?? "",
                    CreatorEmail = user?.Email ?? "",
                    EstActif = e.EstActif
                };
            }).ToList();
        }

        public IEnumerable<EmploiReadDto> GetEmploisActifs()
        {
            var emplois = _context.Emplois.Where(e => e.EstActif && e.DateFin > DateTime.UtcNow).ToList();
            var userIds = emplois.Select(e => e.UserId).Distinct().ToList();
            var users = _userManager.Users.Where(u => userIds.Contains(u.Id)).ToList();

            return emplois.Select(e =>
            {
                var user = users.FirstOrDefault(u => u.Id == e.UserId);
                return new EmploiReadDto
                {
                    Id = e.Id,
                    Titre = e.Titre,
                    Description = e.Description,
                    Entreprise = e.Entreprise,
                    Localisation = e.Localisation,
                    DateDebut = e.DateDebut,
                    DateFin = e.DateFin,
                    ImageUrl = e.ImageUrl,
                    UserId = e.UserId,
                    CreatorName = user?.FullName ?? "",
                    CreatorEmail = user?.Email ?? "",
                    EstActif = e.EstActif
                };
            }).ToList();
        }

        public EmploiReadDto? GetEmploiById(Guid id)
        {
            var e = _context.Emplois.FirstOrDefault(emploi => emploi.Id == id);
            if (e == null) return null;
            var user = _userManager.Users.FirstOrDefault(u => u.Id == e.UserId);
            return new EmploiReadDto
            {
                Id = e.Id,
                Titre = e.Titre,
                Description = e.Description,
                Entreprise = e.Entreprise,
                Localisation = e.Localisation,
                DateDebut = e.DateDebut,
                DateFin = e.DateFin,
                ImageUrl = e.ImageUrl,
                UserId = e.UserId,
                CreatorName = user?.FullName ?? "",
                CreatorEmail = user?.Email ?? "",
                EstActif = e.EstActif
            };
        }

        public Emploi CreateEmploi(EmploiDto dto, string userId)
        {
            var emploi = new Emploi
            {
                Titre = dto.Titre,
                Description = dto.Description,
                Entreprise = dto.Entreprise,
                Localisation = dto.Localisation,
                DateDebut = dto.DateDebut,
                DateFin = dto.DateFin,
                ImageUrl = dto.ImageUrl,
                UserId = userId,
                EstActif = true
            };
            _context.Emplois.Add(emploi);
            _context.SaveChanges();
            return emploi;
        }

        public IEnumerable<EmploiReadDto> GetEmploisByUser(string userId)
        {
            var emplois = _context.Emplois.Where(e => e.UserId == userId).ToList();
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            return emplois.Select(e => new EmploiReadDto
            {
                Id = e.Id,
                Titre = e.Titre,
                Description = e.Description,
                Entreprise = e.Entreprise,
                Localisation = e.Localisation,
                DateDebut = e.DateDebut,
                DateFin = e.DateFin,
                ImageUrl = e.ImageUrl,
                UserId = e.UserId,
                CreatorName = user?.FullName ?? "",
                CreatorEmail = user?.Email ?? "",
                EstActif = e.EstActif
            }).ToList();
        }

        public bool UpdateEmploi(Guid id, EmploiDto dto, string userId)
        {
            var emploi = _context.Emplois.Find(id);
            if (emploi == null || emploi.UserId != userId) return false;
            
            emploi.Titre = dto.Titre;
            emploi.Description = dto.Description;
            emploi.Entreprise = dto.Entreprise;
            emploi.Localisation = dto.Localisation;
            emploi.DateDebut = dto.DateDebut;
            emploi.DateFin = dto.DateFin;
            emploi.ImageUrl = dto.ImageUrl;
            
            _context.SaveChanges();
            return true;
        }

        public bool DeleteEmploi(Guid id, string userId)
        {
            var emploi = _context.Emplois.Find(id);
            if (emploi == null || emploi.UserId != userId) return false;
            
            _context.Emplois.Remove(emploi);
            _context.SaveChanges();
            return true;
        }
    }
}