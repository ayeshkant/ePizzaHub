using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenGeneratorService _tokenGeneratorService;

        public TokenController(ITokenGeneratorService tokenGeneratorService)
        {
            _tokenGeneratorService = tokenGeneratorService;
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
