using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto
{
    public class PostCreateDTO
    {
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
		public IFormFile File { get; set; } = null;
		[Required]
		public int CategoryId { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}
