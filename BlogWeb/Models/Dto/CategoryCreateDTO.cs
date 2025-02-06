using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models.Dto
{
    public class CategoryCreateDTO
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
