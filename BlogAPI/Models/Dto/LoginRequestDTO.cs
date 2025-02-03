using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto
{
    public class LoginRequestDTO
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        public string HashPassword { get; set; }
    }
}
