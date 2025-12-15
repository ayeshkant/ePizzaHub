using ePizzaHub.Application.Contracts;
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
            return Ok();
        }
    }
}
