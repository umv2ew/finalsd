using MediatR;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Commands;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers;

public class RemoveRoomCommandHandler(GameContext context, ILogger<RemovePlayerCommandHandler> logger)
    : IRequestHandler<RemoveRoomCommand, Unit>
{
    private readonly GameContext _context = context;
    private readonly ILogger<RemovePlayerCommandHandler> _logger = logger;

    public async Task<Unit> Handle(RemoveRoomCommand request, CancellationToken cancellationToken)
    {
        var rooms = _context.Rooms.Where(r => r.RoomId == request.RoomId);
        _context.Rooms.RemoveRange(rooms);

        var players = _context.Players.Where(p => p.RoomId == request.RoomId);
        _context.Players.RemoveRange(players);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Players {players} finished a game in room {roomId}",
            players,
            request.RoomId);

        return Unit.Value;
    }
}
