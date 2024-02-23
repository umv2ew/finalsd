using MediatR;

namespace sd.Jatek.Application.Querys;

public class GetNextPlayerQuery(string roomId) : IRequest<bool>
{
    public string RoomId { get; set; } = roomId;
}
