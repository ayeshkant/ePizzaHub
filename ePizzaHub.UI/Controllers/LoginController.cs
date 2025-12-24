using ePizzaHub.UI.Constants;
using ePizzaHub.UI.Models.Response;
using ePizzaHub.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ePizzaHub.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ePizzaApiClient);
                var response=await httpClient.GetFromJsonAsync<ApiResponseModelDto<TokenResponseDto>>($"api/Token/get/{request.UserName}/{request.Password}");

                if (response is not null && response.IsSuccess)
                {
                    var claims=await ProcessToken(response.Data.AccessToken);
                    return RedirectToAction("Index","Home");
                }
            }
            return View(request);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Login");
        }
        private async Task<List<Claim>> ProcessToken(string accessToken)
        {
            var handler= new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);

            var claims = new List<Claim>();

            foreach (var claim in jwtToken.Claims)
            {
                claims.Add(claim);
            }
            await GenerateCookie(claims);

            return claims;
        }
        private async Task GenerateCookie(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties()
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });
        }
    }
}
