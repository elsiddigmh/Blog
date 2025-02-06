using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models.Dto
{
    public class CommentDTO
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }


        // Relationships
        public int PostId { get; set; }
        public PostDTO Post { get; set; } // Navigation Property

        public int UserId { get; set; }
        public UserDTO User { get; set; } // Navigation Property
    }
}
