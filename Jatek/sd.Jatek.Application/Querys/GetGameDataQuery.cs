using MediatR;
using sd.Jatek.Application.ViewModels;

namespace sd.Jatek.Application.Querys;

public class GetGameDataQuery(string roomId) : IRequest<GameDataViewModel>
{
    public string RoomId { get; set; } = roomId;
}
