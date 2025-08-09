using AlumniConnect.API.Models;
using AlumniConnect.API.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AlumniConnect.API.DTOs;




namespace AlumniConnect.API.Services
{
    public class AlumniService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AlumniUser> _userManager;

        public AlumniService(ApplicationDbContext context, UserManager<AlumniUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<AlumniUser> Search(int? promotionId, string? profession)
        {
            var query = _context.Users.Include(u => u.Promotion).AsQueryable();
            if (promotionId.HasValue)
                query = query.Where(u => u.PromotionId == promotionId.Value);
            if (!string.IsNullOrEmpty(profession))
                query = query.Where(u => u.Profession.ToLower().Contains(profession.ToLower()));
            return query.ToList();
        }
        public async Task<List<AlumniDto>> GetAllAlumniExceptAdminsAsync()
        {
            var users = await _context.Users
                .Include(u => u.Promotion)
                .ToListAsync();
            var result = new List<AlumniDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("SuperAdmin"))
                    result.Add(AlumniDto.FromAlumniUser(user));
            }
            return result;
        }
        public async Task<AlumniUser?> GetAlumniByIdAsync(string id)
        {
            return await _userManager.Users
                .Where(u => u.Id == id)
                .Include(u => u.Promotion)
                .FirstOrDefaultAsync();
        }

    }
}
