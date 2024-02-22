using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Commands;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class RemovePlayerCommandHandler : IRequestHandler<RemovePlayerCommand, Unit>
    {
        private readonly GameContext _context;
        private readonly ILogger<RemovePlayerCommandHandler> _logger;

        public RemovePlayerCommandHandler(GameContext context, ILogger<RemovePlayerCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemovePlayerCommand request, CancellationToken cancellationToken)
        {
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId, cancellationToken);

            if (player != null)
                _context.Players.Remove(player);
            else
                _logger.LogError("Player with id: {playerId} doesn't exist",
                request.PlayerId);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Player {playerName} was removed from room: {roomId}",
                player?.PlayerName,
                player?.RoomId);

            return Unit.Value;
        }
    }
}
