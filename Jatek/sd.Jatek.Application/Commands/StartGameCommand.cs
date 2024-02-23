using MediatR;

namespace sd.Jatek.Application.Commands;

public class StartGameCommand(string roomId) : IRequest<Unit>
{
    public string RoomId { get; set; } = roomId;
}
