using CardFile.WebAPI.Models.Request;
using CardFile.WebAPI.Models.Response;
using System.Threading.Tasks;

namespace CardFile.WebAPI.Interfaces
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(Register model);

        Task<UserManagerResponse> LoginUserAsync(Login model);

        Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);

        Task<UserManagerResponse> ForgetPasswordAsync(string email);

        Task<UserManagerResponse> ResetPasswordAsync(ResetPassword model);
    }
}