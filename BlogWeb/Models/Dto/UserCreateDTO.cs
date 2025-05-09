﻿using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models.Dto
{

    public class UserCreateDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        public string HashPassword { get; set; }
        public string Role { get; set; } = "author";

    }

}
