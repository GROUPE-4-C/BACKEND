using AlumniConnect.API.Services;
using AlumniConnect.API.DTOs;
using System;
using System.Collections.Generic;
using AlumniConnect.API.Models;


namespace AlumniConnect.API.Controllers
{
    public class EventsController
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        public IEnumerable<EventReadDto> GetAll() => _eventService.GetAllEvents();

        public EventReadDto? GetById(Guid id) => _eventService.GetEventById(id);

        public Event Create(EventDto dto, string userId) => _eventService.CreateEvent(dto, userId);

        public bool Update(Guid id, EventDto dto, string userId) => _eventService.UpdateEvent(id, dto, userId);

        public bool Delete(Guid id, string userId) => _eventService.DeleteEvent(id, userId);
        public IEnumerable<EventReadDto> GetByUser(string userId) => _eventService.GetEventsByUser(userId);

    }
}
