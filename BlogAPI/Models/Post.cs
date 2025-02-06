using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public bool IsPublished { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime PublishedAt { get; set; }


        // Relationships
        [Required]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
		[JsonIgnore]
		public User Author { get; set; } // Navigation Property

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
		[JsonIgnore]
		public Category Category { get; set; } // Navigation Property
		[JsonIgnore]
		public ICollection<Comment> Comments { get; set; } // One-to-Many
    }
}
