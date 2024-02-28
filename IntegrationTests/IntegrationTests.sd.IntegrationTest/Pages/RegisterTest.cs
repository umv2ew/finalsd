using Bogus;
using FluentAssertions;
using Microsoft.Playwright;
using Xunit;

namespace sd.IntegrationTests.Pages;

[Collection("Test collection")]
public class RegisterTest
{
    private readonly SharedTestContext _testContext;
    private readonly Faker _faker = new();

    public RegisterTest(SharedTestContext testContext)
    {
        _testContext = testContext;
    }

    [Fact]
    public async Task Crate_CreateUser_WhenDataIsValid()
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

        var cookies = await page.Context.CookiesAsync();

        var username = cookies.FirstOrDefault(x => x.Name == "UserName");
        var userId = cookies.FirstOrDefault(x => x.Name == "UserId");

        userId.Should().NotBeNull();
        username.Should().NotBeNull();
        username?.Value.Should().Be(name);

        page.Url.Should().Be("http://localhost/Game/StartGame");
    }

    [Theory]
    [InlineData("name1", "pa", "pa")]
    [InlineData("name2", "password", "passwor")]
    [InlineData("name3", "passwor", "password")]
    public async Task Crate_ShouldReturnToTheSamePage_WhenPasswordIsInvalid(string name, string password, string confirm)
    {
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AuthUrl
        });

        await page.GotoAsync("Account/Register");

        await page.Context.ClearCookiesAsync();

        await page.FillAsync("input[id=formUsername]", name);
        await page.FillAsync("input[id=formPassword]", password);
        await page.FillAsync("input[id=formConfirm]", confirm);

        await page.ClickAsync("button[type=submit]");

        var cookies = await page.Context.CookiesAsync();

        var username = cookies.FirstOrDefault(x => x.Name == "UserName");
        var userId = cookies.FirstOrDefault(x => x.Name == "UserId");

        userId.Should().BeNull();
        username.Should().BeNull();

        page.Url.Should().Be("http://localhost/Account/Register");
    }

    [Fact]
    public async Task Crate_ShouldReturnToTheSamePage_WhenNameIsInvalid()
    {
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AuthUrl
        });

        await page.GotoAsync("Account/Register");

        await page.FillAsync("input[id=formUsername]", "name");
        await page.FillAsync("input[id=formPassword]", "password");
        await page.FillAsync("input[id=formConfirm]", "password");

        await page.ClickAsync("button[type=submit]");
        await page.Context.ClearCookiesAsync();

        await page.GotoAsync("Account/Register");

        await page.FillAsync("input[id=formUsername]", "name");
        await page.FillAsync("input[id=formPassword]", "password");
        await page.FillAsync("input[id=formConfirm]", "password");

        await page.ClickAsync("button[type=submit]");

        var cookies = await page.Context.CookiesAsync();

        var username = cookies.FirstOrDefault(x => x.Name == "UserName");
        var userId = cookies.FirstOrDefault(x => x.Name == "UserId");

        userId.Should().BeNull();
        username.Should().BeNull();

        page.Url.Should().Be("http://localhost/Account/Register");
    }
}
