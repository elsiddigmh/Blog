using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models.Dto
{
    public class CommentCreateDTO
    {
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }


        // Relationships
        [Required]
        public int PostId { get; set; }
        [Required]
        public int UserId { get; set; }
    }


}
