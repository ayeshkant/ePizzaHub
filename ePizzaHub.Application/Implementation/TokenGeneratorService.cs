using AutoMapper;
using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.CustomExceptions;
using ePizzaHub.Application.DTOs.Request;
using ePizzaHub.Application.DTOs.Response;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Implementation
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserTokenService _userTokenService;

        public TokenGeneratorService(IUserService userService, IConfiguration configuration, IUserTokenService userTokenService, IMapper mapper)
        {
            _userService = userService;
            _configuration = configuration;
            _userTokenService = userTokenService;
            _mapper = mapper;
        }

        public async Task<TokenResponseDto> GenerateRefreshTokenAsync(RefreshTokenRequestDto request)
        {
            //check if access token is valid
            var claimsPrincipal = GetTokenClaimPrincipal(request.AccessToken);

            if (claimsPrincipal == null)
                throw new InvalidAccessTokenException("The provided access token is not valid");

            //check if refresh access token is valid
            await ValidatePreviousTokenDetails(claimsPrincipal, request);

            var emailAddress = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var userDetails = await _userService.GetUserAsync(emailAddress);
            return GenerateToken(userDetails);
        }

        public async Task<TokenResponseDto> GenerateToken(string userName, string password)
        {
            var user = await _userService.GetUserAsync(userName);
            if (user == null)
                throw new UserNotFoundException($"The provided email adress {userName} doesn't exist in database.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isPasswordValid)
                throw new InvalidCredentialException("The password provided is invalid");

            var tokenResponseDto= GenerateToken(user);
            if (tokenResponseDto is not null)
            {
                await _userTokenService.AddUserTokenAsync(new UserTokenRequestDto
                {
                    AccessToken = tokenResponseDto.AccessToken,
                    RefreshToken=tokenResponseDto.RefreshToken,
                    UserId=user.Id,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["Jwt:RefreshTokenExpiryInDays"]))
                });
            }
            return tokenResponseDto;
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
                RefreshToken = GenerateRefreshToken()
            };
        }
        private ClaimsPrincipal? GetTokenClaimPrincipal(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            var tokenValidationParameter = new TokenValidationParameters()
            {
                ValidateIssuer=true,
                ValidateAudience=true,
                ValidateLifetime=false,
                ValidateIssuerSigningKey=true,
                ValidIssuer=_configuration["Jwt:Issuer"],
                ValidAudience=_configuration["Jwt:Audience"],
                IssuerSigningKey=new SymmetricSecurityKey(key)
            };
            return tokenHandler.ValidateToken(accessToken, tokenValidationParameter, out _);
        }
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var range = RandomNumberGenerator.Create();
            range.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
        private async Task ValidatePreviousTokenDetails(ClaimsPrincipal principal,RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var previousTokenDetails =
                await FetchPreviousTokenDetails(principal);

            if (previousTokenDetails == null
                || previousTokenDetails.RefreshToken != refreshTokenRequestDto.RefreshToken
                 || previousTokenDetails.AccessToken != refreshTokenRequestDto.AccessToken
                || previousTokenDetails.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new Exception("Invalid Refresh Token Token");
        }

        private async Task<UserTokenResponseDto> FetchPreviousTokenDetails(ClaimsPrincipal principal)
        {
            var userId = principal.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;
            var userTokenResponseDto = await _userTokenService.GetUserTokenAsync(Convert.ToInt32(userId));
            return userTokenResponseDto;
        }
    }
}
