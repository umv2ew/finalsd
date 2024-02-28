using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Microsoft.Playwright;
using Xunit;

namespace sd.IntegrationTests;

public class SharedTestContext : IAsyncLifetime
{
    public const string AuthUrl = "http://localhost";
    private IPlaywright _playwright;
    public IBrowser Browser { get; private set; }

    public static readonly string DockerDirectory =
        Path.GetFullPath(Path.Combine((TemplateString)Directory.GetCurrentDirectory(), @"..\..\..\..\..\"));

    public static readonly string DockerComposeFile =
        Path.GetFullPath(Path.Combine(DockerDirectory, @"docker-compose.yml"));

    public static readonly string DockerComposeOverrideFile =
        Path.GetFullPath(Path.Combine(DockerDirectory, @"docker-compose.override.yml"));

    private readonly ICompositeService _dockerService = new Builder()
        .UseContainer()
        .UseCompose()
        .FromFile(DockerComposeFile)
        .FromFile(DockerComposeOverrideFile)
        .RemoveOrphans()
        .WaitForHttp("test-auth", AuthUrl)
        .Build();

    public async Task InitializeAsync()
    {
        //Directory.SetCurrentDirectory(@"..\..\..");

        _dockerService.Start();
        _playwright = await Playwright.CreateAsync();
        Browser = await _playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 1000
        });
    }

    public async Task DisposeAsync()
    {
        await Browser.DisposeAsync();
        //_dockerService.Dispose();
    }
}
