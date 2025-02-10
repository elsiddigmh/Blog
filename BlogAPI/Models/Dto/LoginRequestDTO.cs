using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto
{
    public class LoginRequestDTO
    {
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
        [Required]
        public string HashPassword { get; set; }
    }
}