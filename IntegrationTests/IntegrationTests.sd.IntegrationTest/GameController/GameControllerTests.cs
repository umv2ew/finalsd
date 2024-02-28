using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using Xunit;

namespace sd.IntegrationTests.GameController;

public class GameControllerTests : IClassFixture<GameApiFactory>
{
    private readonly GameApiFactory _gameApiFactory;

    public GameControllerTests(GameApiFactory apiFactory)
    {
        _gameApiFactory = apiFactory;
    }

    [Fact]
    public async Task Post_ReturnsOk_WhenGameIsStarted()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Game/StartGame");

        var content = new MultipartFormDataContent
        {
            { new StringContent("5"), "Rounds" },
            { new StringContent("True"), "Public" }
        };

        httpRequestMessage.Content = content;
        var header = new ContentDispositionHeaderValue("form-data");
        httpRequestMessage.Content.Headers.ContentDisposition = header;
        var response = await _gameApiFactory.client.SendAsync(httpRequestMessage);

        response.StatusCode.Should().Be(HttpStatusCode.Found);
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_ReturnsToStartGamePage_WhenGameIsStartedWith_NumberOfRoundsBelowRange()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Game/StartGame");

        var content = new MultipartFormDataContent
        {
            { new StringContent("0"), "Rounds" },
            { new StringContent("True"), "Public" }
        };

        httpRequestMessage.Content = content;
        var header = new ContentDispositionHeaderValue("form-data");
        httpRequestMessage.Content.Headers.ContentDisposition = header;

        var responseBelowRange = await _gameApiFactory.client.SendAsync(httpRequestMessage);
        responseBelowRange.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        responseBelowRange.Headers.Location.Should().BeNull();
    }

    [Fact]
    public async Task Post_ReturnsToStartGamePage_WhenGameIsStartedWith_NumberOfRoundsAboveRange()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Game/StartGame");

        var content = new MultipartFormDataContent
        {
            { new StringContent("21"), "Rounds" },
            { new StringContent("True"), "Public" }
        };

        httpRequestMessage.Content = content;
        var header = new ContentDispositionHeaderValue("form-data");
        httpRequestMessage.Content.Headers.ContentDisposition = header;

        var responseAboveRange = await _gameApiFactory.client.SendAsync(httpRequestMessage);
        responseAboveRange.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        responseAboveRange.Headers.Location.Should().BeNull();
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenUserEntersRoom_WithRightId()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Game/StartGame");
        var header = new ContentDispositionHeaderValue("form-data");

        var content = new MultipartFormDataContent
        {
            { new StringContent("5"), "Rounds" },
            { new StringContent("True"), "Public" }
        };

        httpRequestMessage.Content = content;
        httpRequestMessage.Content.Headers.ContentDisposition = header;
        var response = await _gameApiFactory.client.SendAsync(httpRequestMessage);

        var dict = HttpUtility.ParseQueryString(response.Headers.Location?.Query!);
        var id = dict.Get("roomId");
        id.Should().NotBeNullOrEmpty();

        var enterRequestMessage = new HttpRequestMessage(HttpMethod.Get, "Game/EnterRoom/?roomId=" + id);

        enterRequestMessage.Headers.Add("Cookie", "UserName=value1; UserId=value2");
        var enterResponse = await _gameApiFactory.noCookieClient.SendAsync(enterRequestMessage);

        enterResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_ReturnsToStartGamePage_WhenUserEntersRoom_WithWrongId()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Game/StartGame");
        var header = new ContentDispositionHeaderValue("form-data");

        var content = new MultipartFormDataContent
        {
            { new StringContent("5"), "Rounds" },
            { new StringContent("True"), "Public" }
        };

        httpRequestMessage.Content = content;
        httpRequestMessage.Content.Headers.ContentDisposition = header;
        await _gameApiFactory.client.SendAsync(httpRequestMessage);

        var enterRequestMessage = new HttpRequestMessage(HttpMethod.Get, "Game/EnterRoom/?roomId=" + "id");

        enterRequestMessage.Headers.Add("Cookie", "UserName=value1; UserId=value2");
        var enterResponse = await _gameApiFactory.noCookieClient.SendAsync(enterRequestMessage);

        enterResponse?.RequestMessage?.RequestUri.Should().Be(new Uri("http://localhost/Game/StartGame"));
    }

    [Fact]
    public async Task Get_ReturnsToStartGamePage_WhenPlayerIsRemoved()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "Game/StartGame");
        var header = new ContentDispositionHeaderValue("form-data");

        var content = new MultipartFormDataContent
        {
            { new StringContent("5"), "Rounds" },
            { new StringContent("True"), "Public" }
        };

        httpRequestMessage.Content = content;
        httpRequestMessage.Content.Headers.ContentDisposition = header;
        await _gameApiFactory.client.SendAsync(httpRequestMessage);

        var removeRequestMessage = new HttpRequestMessage(HttpMethod.Get, "Game/RemovePlayer");

        var removeResponse = await _gameApiFactory.client.SendAsync(removeRequestMessage);

        removeResponse.StatusCode.Should().Be(HttpStatusCode.Found);
        removeResponse.Headers.Location.Should().Be(new Uri("http://localhost/Game/StartGame"));
    }
}