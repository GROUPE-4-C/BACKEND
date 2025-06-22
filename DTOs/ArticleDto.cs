using System.ComponentModel.DataAnnotations;

namespace AlumniConnect.API.DTOs
{
    public class CreateArticleDto
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        public string Content { get; set; }

        public IFormFile MainImage { get; set; }
    }

    public class UpdateArticleDto
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        public string Content { get; set; }

        public IFormFile MainImage { get; set; }
    }

    public class ArticleResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
} 