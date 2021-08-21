using System.ComponentModel.DataAnnotations;

namespace CardFile.WebAPI.Models
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}