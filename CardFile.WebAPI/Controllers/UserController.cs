using System;
using System.Linq;
using System.Threading.Tasks;
using CardFile.WebAPI.Interfaces;
using CardFile.WebAPI.Models;
using CardFile.WebAPI.Models.Request;
using CardFile.WebAPI.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CardFile.WebAPI.Controllers
{
    // TODO : додивитися як покращити Identity в туторіалах
    // в контроллер, щоб зробити аутентифікацію
    // [Authorize(Role = "Admin")]
    // var userId = User.FindFirst(ClaimTypes.NameIdentifier);

    [Route("api/identity")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService,
                                  IMailService mailService,
                                  IConfiguration configuration)
        {
            _userService = userService;
            _mailService = mailService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });

            var authResponse = await _userService.RegisterUserAsync(model);

            if (!authResponse.Success)
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]UserLoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });

            var authResponse = await _userService.LoginUserAsync(model);
            
            if (!authResponse.Success)
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            
            string content = "<h1>Hey!, new login to your account noticed</h1>" +
                             "<p>New login to your account at " + DateTime.Now + "</p>";

            await _mailService.SendEmailAsync(model.Email, "New login", content);

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId, token);

            if(result.Success)
                return Redirect($"{_configuration["AppUrl"]}/ConfirmEmail.html");

            return BadRequest(result);
        }

        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _userService.ForgetPasswordAsync(email);

            if (result.Success)
                return Ok(result); 

            return BadRequest(result); 
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordRequest model)
        {
            if(ModelState.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(model);

                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }
    }
}