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
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = _tokenService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
