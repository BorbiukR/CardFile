using CardFile.WebAPI.Domain;
using CardFile.WebAPI.Models;
using CardFile.WebAPI.Models.Request;
using System.Threading.Tasks;

namespace CardFile.WebAPI.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationResult> RegisterUserAsync(UserRegistrationRequest model);

        Task<AuthenticationResult> LoginUserAsync(UserLoginRequest model);

        Task<AuthenticationResult> ConfirmEmailAsync(string userId, string token);

        Task<AuthenticationResult> ForgetPasswordAsync(string email);

        Task<AuthenticationResult> ResetPasswordAsync(ResetPasswordRequest model);

        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}