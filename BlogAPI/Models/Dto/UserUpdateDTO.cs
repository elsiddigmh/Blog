using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Dto
{
    public class UserUpdateDTO
    {
        [Required]
        public int Id { get; set; }
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
