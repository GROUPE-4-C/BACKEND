using System.Collections.Generic;
using System.Linq;
using AlumniConnect.API.Models;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Data;
using System;
using Microsoft.AspNetCore.Identity;

namespace AlumniConnect.API.Services
{
    public class EventService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AlumniUser> _userManager;

        public EventService(ApplicationDbContext context, UserManager<AlumniUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<EventReadDto> GetAllEvents()
        {
            var events = _context.Events.ToList();
            var userIds = events.Select(e => e.UserId).Distinct().ToList();
            var users = _userManager.Users.Where(u => userIds.Contains(u.Id)).ToList();

            return events.Select(e =>
            {
                var user = users.FirstOrDefault(u => u.Id == e.UserId);
                return new EventReadDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    Location = e.Location,
                    Organizer = e.Organizer,
                    ImageUrl = e.ImageUrl,
                    UserId = e.UserId,
                    CreatorName = user?.FullName ?? "",
                    CreatorEmail = user?.Email ?? ""
                };
            }).ToList();
        }

        public EventReadDto? GetEventById(Guid id)
        {
            var e = _context.Events.FirstOrDefault(ev => ev.Id == id);
            if (e == null) return null;
            var user = _userManager.Users.FirstOrDefault(u => u.Id == e.UserId);
            return new EventReadDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,
                Organizer = e.Organizer,
                ImageUrl = e.ImageUrl,
                UserId = e.UserId,
                CreatorName = user?.FullName ?? "",
                CreatorEmail = user?.Email ?? ""
            };
        }

        public Event CreateEvent(EventDto dto, string userId)
        {
            var ev = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = DateTime.UtcNow,
                Location = dto.Location,
                Organizer = dto.Organizer,
                ImageUrl = dto.ImageUrl,
                UserId = userId
            };
            _context.Events.Add(ev);
            _context.SaveChanges();
            return ev;
        }

        public IEnumerable<EventReadDto> GetEventsByUser(string userId)
        {
            var events = _context.Events.Where(e => e.UserId == userId).ToList();
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            return events.Select(e => new EventReadDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,
                Organizer = e.Organizer,
                ImageUrl = e.ImageUrl,
                UserId = e.UserId,
                CreatorName = user?.FullName ?? "",
                CreatorEmail = user?.Email ?? ""
            }).ToList();
        }

        public bool UpdateEvent(Guid id, EventDto dto, string userId)
        {
            var ev = _context.Events.Find(id);
            if (ev == null || ev.UserId != userId) return false;
            ev.Title = dto.Title;
            ev.Description = dto.Description;
            ev.Location = dto.Location;
            ev.Organizer = dto.Organizer;
            ev.ImageUrl = dto.ImageUrl;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteEvent(Guid id, string userId)
        {
            var ev = _context.Events.Find(id);
            if (ev == null || ev.UserId != userId) return false;
            _context.Events.Remove(ev);
            _context.SaveChanges();
            return true;
        }
    }
}
