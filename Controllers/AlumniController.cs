using AlumniConnect.API.Services;
using System.Collections.Generic;
using AlumniConnect.API.Models;

namespace AlumniConnect.API.Controllers
{
    public class AlumniController
    {
        private readonly AlumniService _service;
        public AlumniController(AlumniService service)
        {
            _service = service;
        }

        public IEnumerable<AlumniUser> Search(int? promotionId, string? profession)
            => _service.Search(promotionId, profession);
    }
}
