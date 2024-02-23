using MediatR;
using sd.Jatek.Application.ViewModels;

namespace sd.Jatek.Application.Querys;

public class GetWinnerQuery(string roomId) : IRequest<WinnerViewModel>
{
    public string RoomId { get; set; } = roomId;
}
