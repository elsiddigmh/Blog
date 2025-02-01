using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto.Category
{
    public class CategoryUpdateDTO
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
