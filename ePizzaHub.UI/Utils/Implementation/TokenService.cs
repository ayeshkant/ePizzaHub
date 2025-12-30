using ePizzaHub.UI.Constants;
using ePizzaHub.UI.Models.Response;
using ePizzaHub.UI.Utils.Contract;
using System.IdentityModel.Tokens.Jwt;

namespace ePizzaHub.UI.Utils.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public TokenService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public string GetRefreshToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return string.Empty;
            if (context.Request.Cookies.TryGetValue("refresh-token", out var token))
                return token;
            return string.Empty;
        }

        public async Task<string> RefreshTokenAsync()
        {
            var client = _httpClientFactory.CreateClient(ApplicationConstants.ePizzaApiClient);
            var tokens = new TokenResponseDto
            {
                AccessToken = GetToken(),
                RefreshToken = GetRefreshToken()
            };
            var tokenResponse = await client.PostAsJsonAsync<TokenResponseDto>($"api/Token/refresh",tokens);
            tokenResponse.EnsureSuccessStatusCode();

            var response = await tokenResponse.Content.ReadFromJsonAsync<ApiResponseModelDto<TokenResponseDto>>();
            SetAccessToken(response.Data.AccessToken);
            SetRefreshToken(response.Data.RefreshToken);
            return response.Data.AccessToken;
        }

        public string GetToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return string.Empty;
            if (context.Request.Cookies.TryGetValue("access-token", out var token))
                return token;
            return string.Empty;
        }

        public DateTime? GetTokenExpiryTime(string currentToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token=handler.ReadJwtToken(currentToken);
            var expiry = token.Payload.Expiration;

            if (expiry.HasValue)
            {
                var tokenExpiry = DateTimeOffset.FromUnixTimeSeconds(expiry.Value).UtcDateTime;
                return tokenExpiry;
            }
            return null;
        }

        public void SetRefreshToken(string refreshToken)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
            context.Response.Cookies.Append("refresh-token", refreshToken, new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddDays(4)
            });
        }

        public void SetAccessToken(string token)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
            context.Response.Cookies.Append("access-token", token, new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddMinutes(10)
            });
        }
    }
}
