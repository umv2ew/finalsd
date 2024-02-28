using Bogus;
using FluentAssertions;
using Microsoft.Playwright;
using sd.IntegrationTests;
using Xunit;

namespace sd.IntegrationTest.Pages;

[Collection("Test collection")]
public class IndexPage
{
    private readonly SharedTestContext _testContext;
    private readonly Faker _faker = new();

    public IndexPage(SharedTestContext testContext)
    {
        _testContext = testContext;
    }

    [Fact]
    public async Task GameStart_AsAPainter()
    {
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AuthUrl,
        });

        var name = _faker.Person.UserName;

        await page.GotoAsync("Account/Register");


        await page.FillAsync("input[id=formUsername]", name);
        await page.FillAsync("input[id=formPassword]", "password");
        await page.FillAsync("input[id=formConfirm]", "password");

        await page.ClickAsync("button[type=submit]");

        await page.GotoAsync("Game/StartGame");

        await page.FillAsync("input[id=Rounds]", "3");

        await page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();

        var beforeStartGame = await page.Locator("[id=wordToGuess]").IsVisibleAsync();
        beforeStartGame.Should().Be(false);

        await page.GetByRole(AriaRole.Button, new() { Name = "Start Game" }).ClickAsync();

        var afterStartGame = await page.Locator("[id=wordToGuess]").IsVisibleAsync();
        afterStartGame.Should().Be(true);
    }

    [Fact]
    public async Task SendMessage()
    {
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AuthUrl,
        });

        var name = _faker.Person.UserName;

        await page.GotoAsync("Account/Register");


        await page.FillAsync("input[id=formUsername]", name);
        await page.FillAsync("input[id=formPassword]", "password");
        await page.FillAsync("input[id=formConfirm]", "password");

        await page.ClickAsync("button[type=submit]");

        await page.GotoAsync("Game/StartGame");

        await page.FillAsync("input[id=Rounds]", "3");

        await page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Start Game" }).ClickAsync();

        await page.FillAsync("input[id=messageInput]", "test");

        await page.GetByRole(AriaRole.Button, new() { Name = "Send" }).ClickAsync();

        var text = await page.GetByRole(AriaRole.Paragraph).AllTextContentsAsync();

        var message = text.FirstOrDefault(x => x.Contains(name));

        message.Should().NotBeNullOrEmpty();
        message.Should().Be(name + ": test");
    }

    [Fact]
    public async Task GameOver()
    {
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AuthUrl,
        });

        var name = _faker.Person.UserName;

        await page.GotoAsync("Account/Register");


        await page.FillAsync("input[id=formUsername]", name);
        await page.FillAsync("input[id=formPassword]", "password");
        await page.FillAsync("input[id=formConfirm]", "password");

        await page.ClickAsync("button[type=submit]");

        await page.GotoAsync("Game/StartGame");

        await page.FillAsync("input[id=Rounds]", "1");

        await page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Start Game" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Skip" }).ClickAsync();

        SpinWait.SpinUntil(() => page.Url == "http://localhost/Account/Profile",
            5000);

        page.Url.Should().Be("http://localhost/Account/Profile");

        var text = await page.GetByRole(AriaRole.Paragraph).AllTextContentsAsync();

        var message = text.FirstOrDefault();

        message.Should().Be(name);
    }
}
