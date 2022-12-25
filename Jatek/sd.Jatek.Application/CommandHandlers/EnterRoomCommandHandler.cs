using MediatR;
using Microsoft.EntityFrameworkCore;
using sd.Jatek.Application.Commands;
using sd.Jatek.Domain;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class EnterRoomCommandHandler : IRequestHandler<EnterRoomCommand, bool>
    {
        private readonly GameContext _context;
        public EnterRoomCommandHandler(GameContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(EnterRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _context.Rooms.AnyAsync(r => r.RoomId == request.RoomId && !r.Started, cancellationToken);

            var painterInRoom = await _context.Players
                .AnyAsync(p => p.RoomId == request.RoomId && p.PlayerRole == PlayerRole.Painter, cancellationToken);

            if (room)
            {
                await _context.Players.AddAsync(new Player(
                    Guid.NewGuid().ToString(),
                    request.RoomId,
                    0,
                    request.PlayerId,
                    request.PlayerName,
                    painterInRoom ? PlayerRole.Guesser : PlayerRole.Painter
                ), cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }

            return false;
        }
    }
}
