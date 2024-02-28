using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using sd.Jatek.Infrastructure;
using sd.Jatek.Web;
using System.Net;
using Testcontainers.MsSql;
using Xunit;

namespace sd.IntegrationTests.GameController;

public class GameApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    public HttpClient client;
    public HttpClient noCookieClient;

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
            services.RemoveAll(typeof(DbContextOptions<GameContext>));
            services.AddDbContext<GameContext>(options =>
                options.UseSqlServer(
                    $"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True"));

            services.AddSingleton(GetCookieContainerHandler());
        });
    }

    private static CookieContainerHandler GetCookieContainerHandler()
    {
        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Uri("http://localhost"), new Cookie("UserId", "name"));
        cookieContainer.Add(new Uri("http://localhost"), new Cookie("UserName", "name"));

        return new CookieContainerHandler(cookieContainer);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        client = CreateDefaultClient(Services.GetRequiredService<CookieContainerHandler>());
        noCookieClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            HandleCookies = false
        });
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
