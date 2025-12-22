using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.CustomExceptions;
using ePizzaHub.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Implementation
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IUserService _userService;

        public TokenGeneratorService(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<TokenResponseDto> GenerateToken(string userName, string password)
        {
            var user = await _userService.GetUserAsync(userName);
            if (user == null)
                throw new UserNotFoundException($"The provided email adress {userName} doesn't exist in database.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isPasswordValid)
                throw new InvalidCredentialException("The password is invalid");

            return new TokenResponseDto
            {
                AccessToken = "sss",
                RefreshToken = "aaa"
            };
        }
    }
}
