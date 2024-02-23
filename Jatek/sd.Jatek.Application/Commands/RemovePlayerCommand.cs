using MediatR;

namespace sd.Jatek.Application.Commands;

public class RemovePlayerCommand(string playerId) : IRequest<Unit>
{
    public string PlayerId { get; set; } = playerId;
}
