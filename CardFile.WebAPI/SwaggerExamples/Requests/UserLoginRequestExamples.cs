using CardFile.WebAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CardFile.WebAPI.SwaggerExamples.Requests
{
    public class UserLoginRequestExamples : IExamplesProvider<UserLoginRequest>
    {
        public UserLoginRequest GetExamples()
        {
            return new UserLoginRequest
            {
                Email = "orbiuk.r@gmail.com",
                Password = "Aa/1234"
            };
        }
    }
}