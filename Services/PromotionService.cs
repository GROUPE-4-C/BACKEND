using AlumniConnect.API.Models;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniConnect.API.Services
{
    public class PromotionService
    {
        private readonly ApplicationDbContext _context;
        public PromotionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Promotion> GetAll() => _context.Promotions.ToList();

        public Promotion? GetById(int id) => _context.Promotions.Find(id);

        public async Task<Promotion> CreateAsync(PromotionDto dto)
        {
            var promo = new Promotion { Nom = dto.Nom };
            _context.Promotions.Add(promo);
            await _context.SaveChangesAsync();
            return promo;
        }
        public async Task<Promotion?> UpdateAsync(int id, PromotionDto dto)
        {
            var promo = _context.Promotions.Find(id);
            if (promo == null) return null;
            promo.Nom = dto.Nom;
            await _context.SaveChangesAsync();
            return promo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var promo = _context.Promotions.Find(id);
            if (promo == null) return false;
            _context.Promotions.Remove(promo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
