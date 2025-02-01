using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto.User
{
    public class UserCreateDTO
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string Role { get; set; }
    }
}
