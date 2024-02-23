using MediatR;

namespace sd.Jatek.Application.Commands;

public class EnterRoomCommand(string roomId, string playerId, string playerName) : IRequest<bool>
{
    public string RoomId { get; set; } = roomId;
    public string PlayerId { get; set; } = playerId;
    public string PlayerName { get; set; } = playerName;
}
