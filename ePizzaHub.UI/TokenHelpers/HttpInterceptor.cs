using ePizzaHub.UI.Utils.Contract;
using System.Net.Http.Headers;
namespace ePizzaHub.UI.TokenHelpers
{
    public class HttpInterceptor : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public HttpInterceptor(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var accessToken = _tokenService.GetToken();

            if (!string.IsNullOrEmpty(accessToken))
            {
                var tokenExpiryTime = _tokenService.GetTokenExpiryTime(accessToken);
                if (tokenExpiryTime<=DateTime.UtcNow.AddMinutes(2) && request.RequestUri.AbsolutePath != "/api/Token/refresh")
                {
                    await _tokenService.RefreshTokenAsync();
                }
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
