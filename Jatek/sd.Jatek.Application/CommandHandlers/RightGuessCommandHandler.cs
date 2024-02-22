using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Commands;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class RightGuessCommandHandler : IRequestHandler<RightGuessCommand, Unit>
    {
        private readonly GameContext _context;
        private readonly ILogger<RightGuessCommandHandler> _logger;

        public RightGuessCommandHandler(GameContext context, ILogger<RightGuessCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(RightGuessCommand request, CancellationToken cancellationToken)
        {
            var player = await _context.Players
            .FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId && p.RoomId == request.RoomId, cancellationToken);

            if (player != null)
                player.Points++;
            else
                _logger.LogError("There is no player {playerId} in room {roomId}",
                    request.PlayerId,
                    request.RoomId);

            var room = await _context.Rooms
               .FirstOrDefaultAsync(p => p.RoomId == request.RoomId, cancellationToken);

            if (room != null)
                room.RightGuess++;
            else
                _logger.LogError("There is no room with id: {roomId}",
                    request.RoomId);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Player {playerName} in room {roomId} got a point",
                player?.PlayerName,
                request.RoomId);

            return Unit.Value;
        }
    }
}
