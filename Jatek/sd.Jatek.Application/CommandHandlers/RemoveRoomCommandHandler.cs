using MediatR;
using sd.Jatek.Application.Commands;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class RemoveRoomCommandHandler : IRequestHandler<RemoveRoomCommand, Unit>
    {
        private readonly GameContext _context;
        public RemoveRoomCommandHandler(GameContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(RemoveRoomCommand request, CancellationToken cancellationToken)
        {
            var rooms = _context.Rooms.Where(r => r.RoomId == request.RoomId);
            _context.Rooms.RemoveRange(rooms);

            var players = _context.Players.Where(p => p.RoomId == request.RoomId);
            _context.Players.RemoveRange(players);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
