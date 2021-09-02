using System;
using System.Linq;
using System.Threading.Tasks;
using CardFile.WebAPI.Contracts.Request;
using CardFile.WebAPI.Interfaces;
using CardFile.WebAPI.Models;
using CardFile.WebAPI.Models.Request;
using CardFile.WebAPI.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CardFile.WebAPI.Controllers
{
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

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Login to the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequest model)
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

        /// <summary>
        /// Refresh token for user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _userService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        /// <summary>
        /// Confirm user email
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId) )
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId);

            if(result.Success)
                return Redirect($"{_configuration["AppUrl"]}/ConfirmEmail.html");

            return BadRequest(result);
        }

        /// <summary>
        /// Send email to reset password for user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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