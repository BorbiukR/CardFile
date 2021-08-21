using System.Collections.Generic;

namespace CardFile.WebAPI.Models.Response
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}