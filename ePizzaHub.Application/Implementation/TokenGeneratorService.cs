using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.CustomExceptions;
using ePizzaHub.Application.DTOs.Response;
using ePizzaHub.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ePizzaHub.Application.Implementation
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public TokenGeneratorService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        public async Task<TokenResponseDto> GenerateToken(string userName, string password)
        {
            var user = await _userService.GetUserAsync(userName);
            if (user == null)
                throw new UserNotFoundException($"The provided email adress {userName} doesn't exist in database.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isPasswordValid)
                throw new InvalidCredentialException("The password provided is invalid");

            return GenerateToken(user);
        }
        private TokenResponseDto GenerateToken(UserDomain user)
        {
            var secretKey = _configuration["Jwt:Secret"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(ClaimTypes.Name,user.Name),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim("UserId",user.Id.ToString())
                    ]),
                Expires=DateTime.UtcNow.AddMinutes(20),
                SigningCredentials=credentials,
                Issuer= _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResponseDto
            {
                AccessToken = token,
                RefreshToken = "aaa"
            };
        }
        
    }
}
