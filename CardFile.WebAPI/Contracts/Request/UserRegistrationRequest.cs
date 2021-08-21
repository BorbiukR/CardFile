 using System.ComponentModel.DataAnnotations;

namespace CardFile.WebAPI.Models.Request
{
    public class UserRegistrationRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}