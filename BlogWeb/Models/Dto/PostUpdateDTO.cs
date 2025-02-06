using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models.Dto
{
    public class PostUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}
