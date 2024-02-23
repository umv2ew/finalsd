using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Commands;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers;

public class StartGameCommandHandler(GameContext context, ILogger<StartGameCommandHandler> logger)
    : IRequestHandler<StartGameCommand, Unit>
{
    private readonly GameContext _context = context;
    private readonly ILogger<StartGameCommandHandler> _logger = logger;

    public async Task<Unit> Handle(StartGameCommand request, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.RoomId == request.RoomId, cancellationToken);

        if (room != null)
            room.Started = true;
        else
            _logger.LogError("Room with {roomId} doesn't exist",
                request.RoomId);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Game in room {roomId} started",
            request.RoomId);

        return Unit.Value;
    }
}
