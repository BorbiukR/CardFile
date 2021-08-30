using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CardFile.Identity.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this IHttpContextAccessor _httpContextAccessor)
        {
            if (_httpContextAccessor.HttpContext.User == null)
            {
                return string.Empty;
            }

            return _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}