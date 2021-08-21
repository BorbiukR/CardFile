using CardFile.DAL;
using CardFile.WebAPI.Domain;
using CardFile.WebAPI.Interfaces;
using CardFile.WebAPI.Models;
using CardFile.WebAPI.Models.Request;
using CardFile.WebAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly JwtSettings _jwtSettings;
        private readonly CardFileDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public UserService(UserManager<IdentityUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IConfiguration configuration, 
                           IMailService mailService,
                           JwtSettings jwtSettings,
                           CardFileDbContext context,
                           TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mailService = mailService;
            _jwtSettings = jwtSettings;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthenticationResult> RegisterUserAsync(UserRegistrationRequest model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
                return new AuthenticationResult
                {
                    Errors = new[] { "User already exists" },
                    Success = false,
                };

            if (model == null)
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" },
                    Success = false,
                };

            if (model.Password != model.ConfirmPassword)
                return new AuthenticationResult
                {
                    Errors = new[] { "Confirm password doesn't match the password" },
                    Success = false,
                };

            var newUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
            };

            var createdUser = await _userManager.CreateAsync(newUser, model.Password);

            if (!createdUser.Succeeded)
                return new AuthenticationResult
                {
                    Message = "User did not create",
                    Success = false,
                    Errors = createdUser.Errors.Select(e => e.Description)
                };

            //await ConfirmEmailAsync(newUser);

            return await GenerateAuthenticationResultForUserAsync(newUser);     
        }

        public async Task<AuthenticationResult> LoginUserAsync(UserLoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
          
            if (user == null)
                return new AuthenticationResult
                {
                    Errors = new[] { "There is no user with that Email address" },
                    Success = false
                };

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!userHasValidPassword)
                return new AuthenticationResult
                {
                    Errors = new[] { "User/password combination is wrong" },
                    Success = false,
                };

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "User not found"
                };

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
                return new AuthenticationResult
                {
                    Message = "Email confirmed successfully!",
                    Success = true,
                };

            return new AuthenticationResult
            {
                Success = false,
                Message = "Email did not confirm",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<AuthenticationResult> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "No user associated with email",
                };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&token={validToken}";

            string content = "<h1>Follow the instructions to reset your password</h1>" +
                             $"<p>To reset your password <a href='{url}'>Click here</a></p>";

            await _mailService.SendEmailAsync(email, "Reset Password", content);

            return new AuthenticationResult
            {
                Success = true,
                Message = "Reset password URL has been sent to the email successfully!"
            };
        }

        public async Task<AuthenticationResult> ResetPasswordAsync(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "No user associated with email",
                };

            if (model.NewPassword != model.ConfirmPassword)
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string validToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, validToken, model.NewPassword);

            if (result.Succeeded)
                return new AuthenticationResult
                {
                    Message = "Password has been reset successfully!",
                    Success = true,
                };

            return new AuthenticationResult
            {
                Message = "Something went wrong",
                Success = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task ConfirmEmailAsync(IdentityUser newUser)
        {
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = $"{_configuration["AppUrl"]}/api/auth/confirmemail?userid={newUser.Id}&token={validEmailToken}";

            string content = $"<h1>Welcome to Card File Application</h1> <p>Please " +
                             $"confirm your email by <a href='{url}'>Clicking here</a></p>";

            await _mailService.SendEmailAsync(newUser.Email, "Confirm your email", content);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            //var userRoles = await _userManager.GetRolesAsync(user);
            //foreach (var userRole in userRoles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, userRole));
            //    var role = await _roleManager.FindByNameAsync(userRole);
            //    if (role == null) continue;
            //    var roleClaims = await _roleManager.GetClaimsAsync(role);

            //    foreach (var roleClaim in roleClaims)
            //    {
            //        if (claims.Contains(roleClaim))
            //            continue;

            //        claims.Add(roleClaim);
            //    }
            //}

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //var refreshToken = new RefreshToken
            //{
            //    JwtId = token.Id,
            //    UserId = user.Id,
            //    CreationDate = DateTime.UtcNow,
            //    ExpiryDate = DateTime.UtcNow.AddMonths(6)
            //};

            //await _context.RefreshTokens.AddAsync(refreshToken);
            //await _context.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                //RefreshToken = refreshToken.Token
            };
        }      
    }
}