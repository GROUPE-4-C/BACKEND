using AlumniConnect.API.Data;
using AlumniConnect.API.DTOs;
using AlumniConnect.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlumniConnect.API.Services
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AlumniUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ArticleService(
            ApplicationDbContext context,
            UserManager<AlumniUser> userManager,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<ArticleResponseDto> CreateArticleAsync(string userId, CreateArticleDto dto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("L'ID de l'utilisateur est requis");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception($"Utilisateur non trouvé avec l'ID: {userId}");

            if (string.IsNullOrEmpty(dto.Title) || dto.Title.Length < 3)
                throw new Exception("Le titre doit contenir au moins 3 caractères");

            if (string.IsNullOrEmpty(dto.Content) || dto.Content.Length < 10)
                throw new Exception("Le contenu doit contenir au moins 10 caractères");

            string imageUrl = null;
            if (dto.MainImage != null)
            {
                try
                {
                    imageUrl = await SaveImageAsync(dto.MainImage);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erreur lors de la sauvegarde de l'image: {ex.Message}");
                }
            }

            var article = new Article
            {
                Title = dto.Title,
                Content = dto.Content,
                MainImageUrl = imageUrl,
                AuthorId = userId,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la sauvegarde de l'article: {ex.Message}");
            }

            return MapToResponseDto(article, user);
        }

        public async Task<ArticleResponseDto> UpdateArticleAsync(Guid articleId, string userId, UpdateArticleDto dto)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null)
                throw new Exception("Article non trouvé");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Utilisateur non trouvé");

            if (article.AuthorId != userId && !await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                throw new Exception("Non autorisé à modifier cet article");

            string imageUrl = article.MainImageUrl;
            if (dto.MainImage != null)
            {
                imageUrl = await SaveImageAsync(dto.MainImage);
                // Supprimer l'ancienne image si elle existe
                if (!string.IsNullOrEmpty(article.MainImageUrl))
                {
                    DeleteImage(article.MainImageUrl);
                }
            }

            article.Title = dto.Title;
            article.Content = dto.Content;
            article.MainImageUrl = imageUrl;
            article.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToResponseDto(article, user);
        }

        public async Task DeleteArticleAsync(Guid articleId, string userId)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null)
                throw new Exception("Article non trouvé");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Utilisateur non trouvé");

            if (article.AuthorId != userId && !await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                throw new Exception("Non autorisé à supprimer cet article");

            if (!string.IsNullOrEmpty(article.MainImageUrl))
            {
                DeleteImage(article.MainImageUrl);
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleResponseDto>> GetAllArticlesAsync()
        {
            var articles = await _context.Articles
                .Include(a => a.Author)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return articles.Select(a => MapToResponseDto(a, a.Author));
        }

        public async Task<IEnumerable<ArticleResponseDto>> GetUserArticlesAsync(string userId)
        {
            var articles = await _context.Articles
                .Include(a => a.Author)
                .Where(a => a.AuthorId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return articles.Select(a => MapToResponseDto(a, a.Author));
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/uploads/{uniqueFileName}";
        }

        private void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var filePath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private ArticleResponseDto MapToResponseDto(Article article, AlumniUser author)
        {
            return new ArticleResponseDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                MainImageUrl = article.MainImageUrl,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
                AuthorId = article.AuthorId,
                AuthorName = author.FullName
            };
        }
    }
} 