using AlumniConnect.API.Data;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AlumniConnect.API.Services
{
    public class ExperienceService
    {
        private readonly ApplicationDbContext _context;

        public ExperienceService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Récupérer toutes les expériences (publique)
        public async Task<List<ExperienceReadDto>> GetAllExperiencesAsync()
        {
            var experiences = await _context.Experiences
                .Include(e => e.User)
                .OrderByDescending(e => e.DateDebut)
                .ToListAsync();

            return experiences.Select(MapToReadDto).ToList();
        }

        // Récupérer les expériences d'un utilisateur spécifique
        public async Task<List<ExperienceReadDto>> GetUserExperiencesAsync(string userId)
        {
            var experiences = await _context.Experiences
                .Include(e => e.User)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.DateDebut)
                .ToListAsync();

            return experiences.Select(MapToReadDto).ToList();
        }

        // Récupérer une expérience par son ID
        public async Task<ExperienceReadDto?> GetExperienceByIdAsync(Guid id)
        {
            var experience = await _context.Experiences
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            return experience == null ? null : MapToReadDto(experience);
        }

        // Créer une nouvelle expérience
        public async Task<ExperienceReadDto> CreateExperienceAsync(ExperienceDto dto, string userId)
        {
            // Validation : si EnCours = false, DateFin doit être renseignée
            if (!dto.EnCours && dto.DateFin == null)
            {
                throw new ArgumentException("La date de fin est obligatoire si l'expérience n'est pas en cours");
            }

            // Validation : DateFin ne peut pas être antérieure à DateDebut
            if (dto.DateFin.HasValue && dto.DateFin < dto.DateDebut)
            {
                throw new ArgumentException("La date de fin ne peut pas être antérieure à la date de début");
            }

            var experience = new Experience
            {
                Id = Guid.NewGuid(),
                Poste = dto.Poste,
                Entreprise = dto.Entreprise,
                Localisation = dto.Localisation,
                Description = dto.Description,
                DateDebut = dto.DateDebut,
                DateFin = dto.DateFin,
                EnCours = dto.EnCours,
                TypeContrat = dto.TypeContrat,
                Secteur = dto.Secteur,
                UserId = userId,
                DateCreation = DateTime.UtcNow
            };

            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();

            return await GetExperienceByIdAsync(experience.Id)
                ?? throw new InvalidOperationException("Erreur lors de la création de l'expérience");
        }

        // Mettre à jour une expérience
        public async Task<ExperienceReadDto?> UpdateExperienceAsync(Guid id, ExperienceDto dto, string userId)
        {
            var experience = await _context.Experiences.FirstOrDefaultAsync(e => e.Id == id);

            if (experience == null)
                return null;

            // Vérifier que l'utilisateur est le propriétaire
            if (experience.UserId != userId)
                throw new UnauthorizedAccessException("Vous n'êtes pas autorisé à modifier cette expérience");

            // Validation : si EnCours = false, DateFin doit être renseignée
            if (!dto.EnCours && dto.DateFin == null)
            {
                throw new ArgumentException("La date de fin est obligatoire si l'expérience n'est pas en cours");
            }

            // Validation : DateFin ne peut pas être antérieure à DateDebut
            if (dto.DateFin.HasValue && dto.DateFin < dto.DateDebut)
            {
                throw new ArgumentException("La date de fin ne peut pas être antérieure à la date de début");
            }

            // Mettre à jour les propriétés
            experience.Poste = dto.Poste;
            experience.Entreprise = dto.Entreprise;
            experience.Localisation = dto.Localisation;
            experience.Description = dto.Description;
            experience.DateDebut = dto.DateDebut;
            experience.DateFin = dto.DateFin;
            experience.EnCours = dto.EnCours;
            experience.TypeContrat = dto.TypeContrat;
            experience.Secteur = dto.Secteur;
            experience.DateModification = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetExperienceByIdAsync(id);
        }

        // Supprimer une expérience
        public async Task<bool> DeleteExperienceAsync(Guid id, string userId)
        {
            var experience = await _context.Experiences.FirstOrDefaultAsync(e => e.Id == id);

            if (experience == null)
                return false;

            // Vérifier que l'utilisateur est le propriétaire
            if (experience.UserId != userId)
                throw new UnauthorizedAccessException("Vous n'êtes pas autorisé à supprimer cette expérience");

            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
            return true;
        }

        // Récupérer les expériences par entreprise
        public async Task<List<ExperienceReadDto>> GetExperiencesByEntrepriseAsync(string entreprise)
        {
            var experiences = await _context.Experiences
                .Include(e => e.User)
                .Where(e => e.Entreprise.ToLower().Contains(entreprise.ToLower()))
                .OrderByDescending(e => e.DateDebut)
                .ToListAsync();

            return experiences.Select(MapToReadDto).ToList();
        }

        // Récupérer les expériences par secteur
        public async Task<List<ExperienceReadDto>> GetExperiencesBySecteurAsync(string secteur)
        {
            var experiences = await _context.Experiences
                .Include(e => e.User)
                .Where(e => e.Secteur != null && e.Secteur.ToLower().Contains(secteur.ToLower()))
                .OrderByDescending(e => e.DateDebut)
                .ToListAsync();

            return experiences.Select(MapToReadDto).ToList();
        }

        // Récupérer les expériences en cours
        public async Task<List<ExperienceReadDto>> GetCurrentExperiencesAsync()
        {
            var experiences = await _context.Experiences
                .Include(e => e.User)
                .Where(e => e.EnCours)
                .OrderByDescending(e => e.DateDebut)
                .ToListAsync();

            return experiences.Select(MapToReadDto).ToList();
        }

        // Mapper une Experience vers ExperienceReadDto
        private ExperienceReadDto MapToReadDto(Experience experience)
        {
            return new ExperienceReadDto
            {
                Id = experience.Id,
                Poste = experience.Poste,
                Entreprise = experience.Entreprise,
                Localisation = experience.Localisation,
                Description = experience.Description,
                DateDebut = experience.DateDebut,
                DateFin = experience.DateFin,
                EnCours = experience.EnCours,
                TypeContrat = experience.TypeContrat,
                Secteur = experience.Secteur,
                DateCreation = experience.DateCreation,
                DateModification = experience.DateModification,
                UserId = experience.UserId,
                UserFullName = experience.User.FullName,
                UserEmail = experience.User.Email ?? string.Empty,
                UserPhotoUrl = experience.User.PhotoUrl,
                Duree = CalculateDuree(experience.DateDebut, experience.DateFin, experience.EnCours)
            };
        }

        // Calculer la durée d'une expérience
        private string CalculateDuree(DateTime dateDebut, DateTime? dateFin, bool enCours)
        {
            var finEffective = enCours ? DateTime.Now : (dateFin ?? DateTime.Now);
            var duree = finEffective - dateDebut;

            var annees = (int)(duree.TotalDays / 365.25);
            var mois = (int)((duree.TotalDays % 365.25) / 30.44);

            if (annees > 0 && mois > 0)
                return $"{annees} an{(annees > 1 ? "s" : "")} et {mois} mois";
            else if (annees > 0)
                return $"{annees} an{(annees > 1 ? "s" : "")}";
            else if (mois > 0)
                return $"{mois} mois";
            else
                return "Moins d'un mois";
        }
    }
}
