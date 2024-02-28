using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using sd.Auth.Infrastructure;
using sd.Auth.Web;
using Testcontainers.MsSql;
using Xunit;

namespace sd.IntegrationTests.AccountController;
public class AccountApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private const string Database = "master";
    private const string Username = "sa";
    private const string Password = "Pass@word";
    private const ushort MsSqlPort = 1433;

    private readonly MsSqlContainer _dbContainer =
        new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server")
        .WithPortBinding(MsSqlPort)
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithEnvironment("SQLCMDUSER", Username)
        .WithEnvironment("SQLCMDPASSWORD", Password)
        .WithEnvironment("MSSQL_SA_PASSWORD", Password)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var host = _dbContainer.Hostname;
        var port = _dbContainer.GetMappedPublicPort(MsSqlPort);

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AuthContext>));
            services.AddDbContext<AuthContext>(options =>
                options.UseSqlServer(
                    $"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True"));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
