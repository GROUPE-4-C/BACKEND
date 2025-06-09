using AlumniConnect.API.Models;
using AlumniConnect.API.Data;
using System.Collections.Generic;
using System.Linq;

namespace AlumniConnect.API.Services
{
    public class AlumniService
    {
        private readonly ApplicationDbContext _context;
        public AlumniService(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
