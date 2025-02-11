using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models.Dto
{
    public class LoginRequestDTO
    {
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
		[Required]
		[MinLength(5)]
		public string HashPassword { get; set; }
    }
}
