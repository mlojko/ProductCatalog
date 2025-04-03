using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.Models.Health
{
    public class RemoteHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IConfiguration _configuration = configuration;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            using var httpClient = _httpClientFactory.CreateClient();
            try
            {
                var response = await httpClient.GetAsync(_configuration["HealthCheck:ProductCatalogWebUrl"] ?? string.Empty, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy($"Remote endpoints is healthy.");
                }
            }
            catch { }

            return HealthCheckResult.Unhealthy("Remote endpoint is unhealthy");
        }
    }
}
