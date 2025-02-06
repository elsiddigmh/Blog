using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

		// Relationships
		[JsonIgnore]
		public ICollection<Post> Posts { get; set; } // One-to-Many
    }
}
