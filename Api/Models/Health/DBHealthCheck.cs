using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.Models.Health
{
    public class DBHealthCheck(IConfiguration configuration) : IHealthCheck
    {
        private readonly string _connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);

                using var command = new SqlCommand("SELECT 1", connection);
                await command.ExecuteScalarAsync(cancellationToken);

                return HealthCheckResult.Healthy("SQL Server is healthy");
            }
            catch { }

            return HealthCheckResult.Unhealthy("SQL Server is unhealthy");
        }
    }
}
