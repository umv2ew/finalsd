using MediatR;

namespace sd.Jatek.Application.Querys;

public class GetPainterFinishedQuery(string roomId) : IRequest<string>
{
    public string RoomId { get; set; } = roomId;
}
