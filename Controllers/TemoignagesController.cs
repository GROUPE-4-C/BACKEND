using AlumniConnect.API.Services;
using AlumniConnect.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace AlumniConnect.API.Controllers
{
    public class TemoignagesController
    {
        private readonly TemoignageService _service;
        public TemoignagesController(TemoignageService service)
        {
            _service = service;
        }

        public IEnumerable<TemoignageReadDto> GetAll() => _service.GetAll();

        public IEnumerable<TemoignageReadDto> GetByUser(string userId) => _service.GetByUser(userId);

        public async Task<TemoignageReadDto> Create(string userId, TemoignageDto dto) => await _service.CreateAsync(userId, dto);
        public async Task<TemoignageReadDto?> Update(Guid id, string userId, bool isAdmin, TemoignageDto dto)
            => await _service.UpdateAsync(id, userId, isAdmin, dto);

        public async Task<bool> Delete(Guid id, string userId, bool isAdmin) => await _service.DeleteAsync(id, userId, isAdmin);
    }
}
