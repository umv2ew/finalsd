using Bogus;
using FluentAssertions;
using Microsoft.Playwright;
using sd.IntegrationTests;
using System.Web;
using Xunit;

namespace sd.IntegrationTest.Pages;

[Collection("Test collection")]
public class StartGamePage
{
    private readonly SharedTestContext _testContext;
    private readonly Faker _faker = new();

    public StartGamePage(SharedTestContext testContext)
    {
        _testContext = testContext;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    public async Task Create_RoomIsCreated_WhenRightDataIsGiven(int rounds)
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

        await page.FillAsync("input[id=Rounds]", rounds.ToString());

        await page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();

        page.Url.Should().NotBe("http://localhost/Game/StartGame");

        var dict = HttpUtility.ParseQueryString(page.Url[page.Url.IndexOf('?')..]);

        var round = dict.Get("rounds");
        round.Should().NotBeNullOrEmpty();

        var id = dict.Get("roomId");
        id.Should().NotBeNullOrEmpty();

        round.Should().Be(rounds.ToString());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(21)]
    public async Task Create_RoomIsNotCreated_WhenNumberOfRoundsIsOutOfRange(int rounds)
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

        await page.FillAsync("input[id=Rounds]", rounds.ToString());

        await page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();

        page.Url.Should().Be("http://localhost/Game/StartGame");
    }

    [Fact]
    public async Task Enter_EnterRoom_WhenExistingRoomsIdIsGiven()
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

        page.Url.Should().NotBe("http://localhost/Game/StartGame");

        var dict = HttpUtility.ParseQueryString(page.Url[page.Url.IndexOf('?')..]);

        var id = dict.Get("roomId");
        id.Should().NotBeNullOrEmpty();

        var page2 = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AuthUrl,
        });

        await page2.GotoAsync("Account/Register");

        await page2.FillAsync("input[id=formUsername]", name + "32");
        await page2.FillAsync("input[id=formPassword]", "password");
        await page2.FillAsync("input[id=formConfirm]", "password");

        await page2.ClickAsync("button[type=submit]");

        await page2.GotoAsync("Game/StartGame");

        await page2.FillAsync("input[id=groupId]", id);

        await page2.GetByRole(AriaRole.Button, new() { Name = "Enter" }).ClickAsync();

        page2.Url.Should().NotBe("http://localhost/Game/StartGame");
    }
}
