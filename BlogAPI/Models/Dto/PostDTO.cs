using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto
{
    public class PostDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
		public string PhotoUrl { get; set; }
		public bool IsPublished { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime PublishedAt { get; set; }


        // Relationships
        public int AuthorId { get; set; }
        public User Author { get; set; } // Navigation Property

        public int CategoryId { get; set; }
        public Category Category { get; set; } // Navigation Property

        public ICollection<Comment> Comments { get; set; } // One-to-Many
    }
}
