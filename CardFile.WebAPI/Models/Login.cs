﻿using System.ComponentModel.DataAnnotations;

namespace CardFile.WebAPI.Models
{
    public class Login
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
    }
}