using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace sd.IntegrationTests.AccountController;

public class AccountControllerTest : IClassFixture<AccountApiFactory>
{
    private readonly HttpClient _client;

    public AccountControllerTest(AccountApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async void Post_ShouldReturnToStartGamePage_WhenUserRegisterWithRightData()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Register");

        var content = new MultipartFormDataContent
        {
            { new StringContent("Name"), "Username" },
            { new StringContent("Password"), "Password" },
            { new StringContent("Password"), "ConfirmPassword" }
        };

        httpRequestMessage.Content = content;
        var header = new ContentDispositionHeaderValue("form-data");
        httpRequestMessage.Content.Headers.ContentDisposition = header;

        var response = await _client.SendAsync(httpRequestMessage);

        SpinWait.SpinUntil(() => response?.RequestMessage?.RequestUri == new Uri("http://localhost/Game/StartGame"),
            5000);

        response?.RequestMessage?.RequestUri.Should().Be(new Uri("http://localhost/Game/StartGame"));
    }

    [Fact]
    public async void Post_ShouldReturnOK_WHenUserRegisterWithWrongPassword()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Register");

        var content = new MultipartFormDataContent
        {
            { new StringContent("Name"), "Username" },
            { new StringContent("Passwor"), "Password" },
            { new StringContent("Password"), "ConfirmPassword" }
        };

        httpRequestMessage.Content = content;
        var header = new ContentDispositionHeaderValue("form-data");
        httpRequestMessage.Content.Headers.ContentDisposition = header;

        var response = await _client.SendAsync(httpRequestMessage);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        response?.RequestMessage?.RequestUri.Should().Be(new Uri("http://localhost/Account/Register"));
    }

    [Fact]
    public async void Post_ShouldReturnOK_WHenUserRegisterWithInvalidPassword()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Register");

        var content = new MultipartFormDataContent
        {
            { new StringContent("Name"), "Username" },
            { new StringContent("Pa"), "Password" },
            { new StringContent("Pa"), "ConfirmPassword" }
        };

        httpRequestMessage.Content = content;
        var header = new ContentDispositionHeaderValue("form-data");
        httpRequestMessage.Content.Headers.ContentDisposition = header;

        var response = await _client.SendAsync(httpRequestMessage);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        response?.RequestMessage?.RequestUri.Should().Be(new Uri("http://localhost/Account/Register"));
    }

    [Fact]
    public async void Post_ShouldReturnFound_WhenUserLoginWithRightData()
    {
        var header = new ContentDispositionHeaderValue("form-data");
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Register");

        var content = new MultipartFormDataContent
        {
            { new StringContent("Name"), "Username" },
            { new StringContent("Password"), "Password" },
            { new StringContent("Password"), "ConfirmPassword" }
        };

        httpRequestMessage.Content = content;
        httpRequestMessage.Content.Headers.ContentDisposition = header;
        await _client.SendAsync(httpRequestMessage);

        var loginRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Login");

        var loginContent = new MultipartFormDataContent
        {
            { new StringContent("Name"), "Username" },
            { new StringContent("Password"), "Password" },
        };

        loginRequestMessage.Content = loginContent;
        loginRequestMessage.Content.Headers.ContentDisposition = header;
        var response = await _client.SendAsync(loginRequestMessage);

        response?.RequestMessage?.RequestUri.Should().Be(new Uri("http://localhost/Game/StartGame"));
    }

    [Fact]
    public async void Post_ShouldReturnOk_WhenUserLoginWithWrongUsername()
    {
        var header = new ContentDispositionHeaderValue("form-data");
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Register");

        var content = new MultipartFormDataContent
        {
            { new StringContent("Name"), "Username" },
            { new StringContent("Password"), "Password" },
            { new StringContent("Password"), "ConfirmPassword" }
        };

        httpRequestMessage.Content = content;
        httpRequestMessage.Content.Headers.ContentDisposition = header;
        await _client.SendAsync(httpRequestMessage);

        var loginRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Login");

        var loginContent = new MultipartFormDataContent
        {
            { new StringContent("NotTheName"), "Username" },
            { new StringContent("Password"), "Password" },
        };

        loginRequestMessage.Content = loginContent;
        loginRequestMessage.Content.Headers.ContentDisposition = header;
        var response = await _client.SendAsync(loginRequestMessage);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response?.RequestMessage?.RequestUri.Should().Be(new Uri("http://localhost/Account/Login"));
    }

    [Fact]
    public async void Post_ShouldReturnOk_WhenUserLoginWithWrongPassword()
    {
        var header = new ContentDispositionHeaderValue("form-data");
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Register");

        var content = new MultipartFormDataContent
        {
            { new StringContent("Name"), "Username" },
            { new StringContent("Password"), "Password" },
            { new StringContent("Password"), "ConfirmPassword" }
        };

        httpRequestMessage.Content = content;
        httpRequestMessage.Content.Headers.ContentDisposition = header;
        await _client.SendAsync(httpRequestMessage);

        var loginRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Login");

        var loginContent = new MultipartFormDataContent
        {
            { new StringContent("Name"), "Username" },
            { new StringContent("NotThePassword"), "Password" },
        };

        loginRequestMessage.Content = loginContent;
        loginRequestMessage.Content.Headers.ContentDisposition = header;
        var response = await _client.SendAsync(loginRequestMessage);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response?.RequestMessage?.RequestUri.Should().Be(new Uri("http://localhost/Account/Login"));
    }

    [Fact]
    public async void Post_ShouldReturnOK_WhenUserLogout()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/Register");

        var content = new MultipartFormDataContent
        {
            { new StringContent("Name"), "Username" },
            { new StringContent("Password"), "Password" },
            { new StringContent("Password"), "ConfirmPassword" }
        };

        httpRequestMessage.Content = content;
        var header = new ContentDispositionHeaderValue("form-data");
        httpRequestMessage.Content.Headers.ContentDisposition = header;

        await _client.SendAsync(httpRequestMessage);

        var logoutHttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "Account/Logout");
        var response = await _client.SendAsync(logoutHttpRequestMessage);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
