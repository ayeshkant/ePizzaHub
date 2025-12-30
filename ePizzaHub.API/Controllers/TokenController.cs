using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Request;
using ePizzaHub.Application.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IUserTokenService _userTokenService;

        public TokenController(ITokenGeneratorService tokenGeneratorService, IUserTokenService userTokenService)
        {
            _tokenGeneratorService = tokenGeneratorService;
            _userTokenService = userTokenService;
        }
        [HttpGet]
        [Route("get/{userName}/{password}")]
        public async Task<IActionResult> GetToken(string userName, string password)
        {
            var response=await _tokenGeneratorService.GenerateToken(userName, password);

            return Ok(response);
        }
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var response = await _tokenGeneratorService.GenerateRefreshTokenAsync(request);

            return Ok(response);
        }
    }
}
