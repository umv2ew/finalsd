using Bogus;
using FluentAssertions;
using Microsoft.Playwright;
using sd.IntegrationTests;
using Xunit;

namespace sd.IntegrationTest.Pages;

[Collection("Test collection")]
public class LoginTest
{
    private readonly SharedTestContext _testContext;
    private readonly Faker _faker = new();

    public LoginTest(SharedTestContext testContext)
    {
        _testContext = testContext;
    }

    [Fact]
    public async Task Login_LoginUser_WhenRightDataIsGiven()
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

        await page.Context.ClearCookiesAsync();

        await page.GotoAsync("Account/Login");

        await page.FillAsync("input[id=fromUserName]", name);
        await page.FillAsync("input[id=formpassword]", "password");

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
    [InlineData("name1", "password")]
    [InlineData("loginUserName", "passwor")]
    public async Task Login_ShouldReturnToTheSamePage_WhenDataIsInvalid(string name, string password)
    {
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AuthUrl,
        });

        await page.GotoAsync("Account/Register");

        await page.FillAsync("input[id=formUsername]", "loginUserName");
        await page.FillAsync("input[id=formPassword]", "password");
        await page.FillAsync("input[id=formConfirm]", "password");

        await page.ClickAsync("button[type=submit]");

        await page.Context.ClearCookiesAsync();

        await page.GotoAsync("Account/Login");

        await page.FillAsync("input[id=fromUserName]", name);
        await page.FillAsync("input[id=formpassword]", password);

        await page.ClickAsync("button[type=submit]");

        var cookies = await page.Context.CookiesAsync();

        var username = cookies.FirstOrDefault(x => x.Name == "UserName");
        var userId = cookies.FirstOrDefault(x => x.Name == "UserId");

        userId.Should().BeNull();
        username.Should().BeNull();

        page.Url.Should().Be("http://localhost/Account/Login");
    }
}
