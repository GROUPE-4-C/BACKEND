using AlumniConnect.API.Models;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AlumniConnect.API.Services
{
    public class TemoignageService
    {
        private readonly ApplicationDbContext _context;
        public TemoignageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TemoignageReadDto> GetAll()
        {
            return _context.Temoignages
                .Include(t => t.User)
                .ThenInclude(u => u.Promotion)
                .OrderByDescending(t => t.Date)
                .Select(t => new TemoignageReadDto
                {
                    Id = t.Id,
                    Contenu = t.Contenu,
                    Date = t.Date,
                    UserId = t.UserId,
                    FullName = t.User.FullName,
                    Email = t.User.Email,
                    Profession = t.User.Profession,
                    PhotoUrl = t.User.PhotoUrl,
                    Promotion = t.User.Promotion != null ? t.User.Promotion.Nom : null
                })
                .ToList();
        }

        public IEnumerable<TemoignageReadDto> GetByUser(string userId)
        {
            return _context.Temoignages
                .Include(t => t.User)
                .ThenInclude(u => u.Promotion)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .Select(t => new TemoignageReadDto
                {
                    Id = t.Id,
                    Contenu = t.Contenu,
                    Date = t.Date,
                    UserId = t.UserId,
                    FullName = t.User.FullName,
                    Email = t.User.Email,
                    Profession = t.User.Profession,
                    PhotoUrl = t.User.PhotoUrl,
                    Promotion = t.User.Promotion != null ? t.User.Promotion.Nom : null
                })
                .ToList();
        }

        public async Task<TemoignageReadDto> CreateAsync(string userId, TemoignageDto dto)
        {
            var temoignage = new Temoignage { UserId = userId, Contenu = dto.Contenu };
            _context.Temoignages.Add(temoignage);
            await _context.SaveChangesAsync();

            // Recharge avec navigation pour DTO enrichi
            temoignage = await _context.Temoignages
                .Include(t => t.User)
                .ThenInclude(u => u.Promotion)
                .FirstOrDefaultAsync(t => t.Id == temoignage.Id);

            return new TemoignageReadDto
            {
                Id = temoignage.Id,
                Contenu = temoignage.Contenu,
                Date = temoignage.Date,
                UserId = temoignage.UserId,
                FullName = temoignage.User.FullName,
                Email = temoignage.User.Email,
                Profession = temoignage.User.Profession,
                PhotoUrl = temoignage.User.PhotoUrl,
                Promotion = temoignage.User.Promotion != null ? temoignage.User.Promotion.Nom : null
            };
        }
        public async Task<TemoignageReadDto?> UpdateAsync(Guid id, string userId, bool isAdmin, TemoignageDto dto)
        {
            var temoignage = await _context.Temoignages
                .Include(t => t.User)
                .ThenInclude(u => u.Promotion)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (temoignage == null) return null;
            if (!isAdmin && temoignage.UserId != userId) return null;

            temoignage.Contenu = dto.Contenu;
            await _context.SaveChangesAsync();

            return new TemoignageReadDto
            {
                Id = temoignage.Id,
                Contenu = temoignage.Contenu,
                Date = temoignage.Date,
                UserId = temoignage.UserId,
                FullName = temoignage.User.FullName,
                Email = temoignage.User.Email,
                Profession = temoignage.User.Profession,
                PhotoUrl = temoignage.User.PhotoUrl,
                Promotion = temoignage.User.Promotion != null ? temoignage.User.Promotion.Nom : null
            };
        }


        public async Task<bool> DeleteAsync(Guid id, string userId, bool isAdmin)
        {
            var temoignage = await _context.Temoignages.FindAsync(id);
            if (temoignage == null) return false;
            if (!isAdmin && temoignage.UserId != userId) return false;
            _context.Temoignages.Remove(temoignage);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
