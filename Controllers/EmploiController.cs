// EmploisController.cs - Contrôleur simplifié
using AlumniConnect.API.Services;
using AlumniConnect.API.DTOs;
using System;
using System.Collections.Generic;
using AlumniConnect.API.Models;

namespace AlumniConnect.API.Controllers
{
    public class EmploisController
    {
        private readonly EmploiService _emploiService;

        public EmploisController(EmploiService emploiService)
        {
            _emploiService = emploiService;
        }

        public IEnumerable<EmploiReadDto> GetAll() => _emploiService.GetAllEmplois();

        public EmploiReadDto? GetById(Guid id) => _emploiService.GetEmploiById(id);

        public Emploi Create(EmploiDto dto, string userId) => _emploiService.CreateEmploi(dto, userId);

        public bool Update(Guid id, EmploiDto dto, string userId) => _emploiService.UpdateEmploi(id, dto, userId);

        public bool Delete(Guid id, string userId) => _emploiService.DeleteEmploi(id, userId);

        public IEnumerable<EmploiReadDto> GetByUser(string userId) => _emploiService.GetEmploisByUser(userId);

        public IEnumerable<EmploiReadDto> GetActifs() => _emploiService.GetEmploisActifs();
    }
}