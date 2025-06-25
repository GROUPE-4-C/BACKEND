using AlumniConnect.API.Models;
using AlumniConnect.API.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



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
            var query = _context.Users.AsQueryable();
            if (promotionId.HasValue)
                query = query.Where(u => u.PromotionId == promotionId.Value);
            if (!string.IsNullOrEmpty(profession))
                query = query.Where(u => u.Profession.ToLower().Contains(profession.ToLower()));
            return query.ToList();
        }
        public async Task<List<AlumniUser>> GetAllAlumniExceptAdminsAsync()
        {
            var users = _context.Users.ToList();
            var result = new List<AlumniUser>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("SuperAdmin"))
                    result.Add(user);
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
