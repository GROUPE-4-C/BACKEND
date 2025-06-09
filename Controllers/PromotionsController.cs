using AlumniConnect.API.Services;
using AlumniConnect.API.DTOs;
using System.Collections.Generic;
using AlumniConnect.API.Models;

namespace AlumniConnect.API.Controllers
{
    public class PromotionsController
    {
        private readonly PromotionService _service;
        public PromotionsController(PromotionService service)
        {
            _service = service;
        }

        public IEnumerable<Promotion> GetAll() => _service.GetAll();

        public Promotion? GetById(int id) => _service.GetById(id);

        public async Task<Promotion> Create(PromotionDto dto) => await _service.CreateAsync(dto);

        public async Task<bool> Delete(int id) => await _service.DeleteAsync(id);
        public async Task<Promotion?> Update(int id, PromotionDto dto)
            => await _service.UpdateAsync(id, dto);

    }
}
