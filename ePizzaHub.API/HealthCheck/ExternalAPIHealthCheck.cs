using Azure;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ePizzaHub.API.HealthCheck
{
    public class ExternalAPIHealthCheck : IHealthCheck
    {
        private readonly HttpClient _client;

        public ExternalAPIHealthCheck(HttpClient client)
        {
            _client = client;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _client.GetAsync("https://reqres.in/api/users?page=2");
                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("Regress API is running");
                }
                return HealthCheckResult.Degraded($"Regress API returned status code: {response.StatusCode }");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Regress API is unreachable: {ex.Message}");
            }
            throw new NotImplementedException();
        }
    }
}
