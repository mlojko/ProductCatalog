using System.Net;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.Models.Health
{
    public static class HealthCheckExtensions
    {
        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("API Self", () => HealthCheckResult.Healthy(), ["Feedback", "Basic"])
                .AddCheck<DBHealthCheck>("SQL Server", failureStatus: HealthStatus.Unhealthy, tags: ["Feedback", "Database"])
                .AddCheck<RemoteHealthCheck>("ProductCatalog UI Check", failureStatus: HealthStatus.Unhealthy, ["Feedback", "UI", "Remote"])
                .AddCheck<MemoryHealthCheck>($"Memory Check", failureStatus: HealthStatus.Unhealthy, tags: ["Feedback", "Memory", "Service"]);

            var hcProtocol = configuration["HealthCheck:HealthCheckEndpointProtocol"] ?? "http";
            var hcHost = configuration["HealthCheck:HealthCheckEndpointHost"] ?? Dns.GetHostName();
            var hcPort = configuration["HealthCheck:HealthCheckEndpointPort"] ?? "8080";


            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10); //time in seconds between check    
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
                opt.SetApiMaxActiveRequests(1); //api requests concurrency    
                opt.AddHealthCheckEndpoint("Feedback API", $"{hcProtocol}://{hcHost}:{hcPort}/api/health"); //map health check api    

            }).AddInMemoryStorage();
        }
    }
}
