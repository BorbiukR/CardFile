using System.ComponentModel.DataAnnotations;

namespace CardFile.WebAPI.Contracts.Request
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }

        [Required(ErrorMessage = "RefreshToken is required")]
        public string RefreshToken { get; set; }
    }
}