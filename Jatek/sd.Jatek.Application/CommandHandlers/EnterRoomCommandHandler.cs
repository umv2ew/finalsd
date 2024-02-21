using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Commands;
using sd.Jatek.Domain;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class EnterRoomCommandHandler : IRequestHandler<EnterRoomCommand, bool>
    {
        private readonly GameContext _context;
        private readonly ILogger<EnterRoomCommandHandler> _logger;

        public EnterRoomCommandHandler(GameContext context, ILogger<EnterRoomCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(EnterRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _context.Rooms.AnyAsync(r => r.RoomId == request.RoomId && !r.Started, cancellationToken);

            var painterInRoom = await _context.Players
                .AnyAsync(p => p.RoomId == request.RoomId && p.PlayerRole == PlayerRole.Painter, cancellationToken);

            var alreadyInRoom = _context.Players.FirstOrDefault(p => p.PlayerId == request.PlayerId);

            if (alreadyInRoom != null)
            {
                _context.Players.Remove(alreadyInRoom);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Player: {playerName} was removed from room: {roomId}",
                    alreadyInRoom.PlayerName,
                    alreadyInRoom.RoomId);
            }

            if (room)
            {
                var maxPlace = _context.Players.Where(p => p.RoomId == request.RoomId).Select(p => p.Place).Max();
                await _context.Players.AddAsync(new Player(
                    Guid.NewGuid().ToString(),
                    request.RoomId,
                    0,
                    request.PlayerId,
                    request.PlayerName,
                    maxPlace + 1,
                    painterInRoom ? PlayerRole.Guesser : PlayerRole.Painter
                ), cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Player: {playerName} entered room: {roomId}",
                    request.PlayerName,
                    request.RoomId);

                return true;
            }

            _logger.LogInformation("A room with the id: {roomId} doesn't exist",
                request.RoomId);

            return false;
        }
    }
}
