using ePizzaHub.UI.Utils.Contract;

namespace ePizzaHub.UI.Utils.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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

        public void SetRefreshToken(string refreshToken)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
        }

        public void SetToken(string token)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
            context.Response.Cookies.Append("access-token", token, new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            });
        }
    }
}
