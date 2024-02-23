using MediatR;

namespace sd.Jatek.Application.Querys;

public class GetRoleByIdQuery(string playerId, string roomId) : IRequest<string>
{
    public string PlayerId { get; set; } = playerId;
    public string RoomId { get; set; } = roomId;
}
