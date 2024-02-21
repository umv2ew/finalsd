
using sd.Jatek.Application.Dtos;
using sd.Jatek.Application.ViewModels;
using sd.Jatek.Web.Helpers;
using System.Text;

namespace sd.Jatek.Web.Models;

public class GameService : IGameService
{
    private readonly HttpClient _client;
    public const string BasePath = "http://localhost:5261/api/";

    public GameService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<bool> EnterRoom(string roomId, string userId, string userName)
    {
        var response = await _client.GetAsync(BasePath + $"EnterRoom/{roomId}/{userId}/{userName}");

        var result = await response.ReadStringAsync<string>();

        return result == "true";
    }

    public async Task<string> GetRole(string id, string playerId)
    {
        var response = await _client.GetAsync(BasePath + $"GetRole/{id}");

        return await response.ReadStringAsync<string>();
    }

    public async Task<List<PublicRoomsViewModel>> PublicRooms()
    {
        var response = await _client.GetAsync(BasePath + $"PublicRooms");

        return await response.ReadContentAsync<List<PublicRoomsViewModel>>();
    }

    public async Task CreateRoom(RoomDto dto)
    {
        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);

        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

        await _client.PostAsync(BasePath + $"CreateRoom", httpContent);
    }

    public async Task<string> PainterFinished(string id)
    {
        var response = await _client.GetAsync(BasePath + $"PainterFinished/{id}");

        return await response.ReadStringAsync<string>();
    }

    public async Task RemovePlayer(string id)
    {
        await _client.PostAsync(BasePath + $"RemovePlayer/{id}", null);
    }
}
