using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace sd.ApiGateway.Web;

public class ApiGatewayHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using HttpClient client = new();
        Uri siteUri = new("http://172.17.0.1/Account/Login");

        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, siteUri), cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return HealthCheckResult.Healthy("A healthy result.");
        }

        return new HealthCheckResult(context.Registration.FailureStatus, "An unhealthy result.");
    }
}
