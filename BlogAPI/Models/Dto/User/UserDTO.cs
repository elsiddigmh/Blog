using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto.User
{
    public class UserDTO
    {
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
        public ICollection<Post> Posts { get; set; } // One-to-Many
        public ICollection<Comment> Comments { get; set; } // One-to-Many
    }

}
