using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Querys;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers;

public class GetPainterFinishedQueryHandler(GameContext context, ILogger<GetPainterFinishedQueryHandler> logger)
    : IRequestHandler<GetPainterFinishedQuery, string>
{
    private readonly GameContext _context = context;
    private readonly ILogger<GetPainterFinishedQueryHandler> _logger = logger;

    public async Task<string> Handle(GetPainterFinishedQuery request, CancellationToken cancellationToken)
    {
        var players = _context.Players
            .Where(p => p.RoomId == request.RoomId)
            .Count();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.RoomId == request.RoomId, cancellationToken);

        if (room != null && room.RightGuess == players - 1)
        {
            room.RightGuess = 0;
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Painter finished painting in room: {roomId}", request.RoomId);
            return "true";
        }

        return "false";
    }
}
