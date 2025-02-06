using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models.Dto
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Slug { get; set; }

        // Relationships
        public ICollection<PostDTO> Posts { get; set; } // One-to-Many
    }
}
