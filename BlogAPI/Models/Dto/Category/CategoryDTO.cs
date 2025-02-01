using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Slug { get; set; }

        // Relationships
        public ICollection<Post> Posts { get; set; } // One-to-Many
    }
}
