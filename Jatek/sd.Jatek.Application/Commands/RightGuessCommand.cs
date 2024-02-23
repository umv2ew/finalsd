using MediatR;

namespace sd.Jatek.Application.Commands;

public class RightGuessCommand(string roomId, string playerId) : IRequest<Unit>
{
    public string RoomId { get; set; } = roomId;
    public string PlayerId { get; set; } = playerId;
}
