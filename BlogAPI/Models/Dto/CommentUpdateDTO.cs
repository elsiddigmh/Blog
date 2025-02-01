using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto
{
    public class CommentUpdateDTO
    {
        [Required]
        public int Id { get; set; }
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
