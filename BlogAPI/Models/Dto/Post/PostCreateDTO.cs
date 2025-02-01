using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto.Post
{
    public class PostCreateDTO
    {
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}
