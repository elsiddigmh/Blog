using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models.Dto
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relationships
        public ICollection<PostDTO> Posts { get; set; } // One-to-Many
        public ICollection<CommentDTO> Comments { get; set; } // One-to-Many
    }
}
