using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Querys;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers;

public class GetNextPlayerQueryHandler(GameContext context, ILogger<GetNextPlayerQueryHandler> logger)
    : IRequestHandler<GetNextPlayerQuery, bool>
{
    private readonly GameContext _context = context;
    private readonly ILogger<GetNextPlayerQueryHandler> _logger = logger;

    public async Task<bool> Handle(GetNextPlayerQuery request, CancellationToken cancellationToken)
    {
        var players = await _context.Players
            .Where(p => p.RoomId == request.RoomId)
            .Select(p => p.PlayerId)
            .ToListAsync(cancellationToken);

        if (players.Count > 1)
        {
            var painter = await _context.Players
                .FirstOrDefaultAsync(p => p.RoomId == request.RoomId && p.PlayerRole == Domain.PlayerRole.Painter, cancellationToken)
                ?? throw new Exception("There is no painter");

            var orderByMax = _context.Players
                .Where(p => p.RoomId == request.RoomId)
                .OrderBy(p => p.Place);

            string nextPainterId = "";

            if (painter != null && painter.Place == orderByMax.Last().Place)
                nextPainterId = orderByMax
                    .First()
                    .PlayerId;
            else
                nextPainterId = orderByMax
                    .First(pl => pl.Place > painter!.Place)
                    .PlayerId;

            var guesser = await _context.Players
                .FirstOrDefaultAsync(p => p.RoomId == request.RoomId && p.PlayerId == painter!.PlayerId, cancellationToken);

            if (guesser != null)
                guesser.PlayerRole = Domain.PlayerRole.Guesser;

            var nextPainter = await _context.Players
                .FirstOrDefaultAsync(p => p.RoomId == request.RoomId && p.PlayerId == nextPainterId, cancellationToken);

            if (nextPainter != null)
                nextPainter.PlayerRole = Domain.PlayerRole.Painter;

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == request.RoomId, cancellationToken);

            var roundOver = nextPainterId == orderByMax
                .First().PlayerId;

            if (room != null)
            {
                if (roundOver)
                    room.Rounds--;

                room.RightGuess = 0;
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogDebug("The next painter in room: {roomId} is {playerId}", request.RoomId, nextPainterId);
            return roundOver;
        }
        else
        {
            _logger.LogDebug("There is 0 or 1 player in room {roomId}", request.RoomId);
            return true;
        }
    }
}
