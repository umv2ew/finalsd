using sd.Jatek.Application.Dtos;
using sd.Jatek.Application.ViewModels;

namespace sd.Jatek.Web.Models;

public interface IGameService
{
    Task<string> GetRole(string id, string playerId);

    Task<bool> EnterRoom(string roomId, string userId, string userName);

    Task<List<PublicRoomsViewModel>> PublicRooms();

    Task CreateRoom(RoomDto dto);

    Task<string> PainterFinished(string id);

    Task RemovePlayer(string id);

}
