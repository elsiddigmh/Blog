using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto
{
    public class PostUpdateDTO
    {
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
    }
}
