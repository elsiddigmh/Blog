using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto
{
    public class CategoryUpdateDTO
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
