using MediatR;
using sd.Jatek.Application.Querys;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers
{
    public class RightGuessQueryHandler : IRequestHandler<RightGuessQuery, bool>
    {
        private readonly GameContext _context;
        public RightGuessQueryHandler(GameContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(RightGuessQuery request, CancellationToken cancellationToken)
        {
            _context.Players
               .FirstOrDefault(p => p.PlayerId == request.PlayerId && p.RoomId == request.RoomId)
               .Points++;

            _context.Rooms
               .FirstOrDefault(p => p.RoomId == request.RoomId)
               .RightGuess++;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
            /*
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == request.RoomId);

            var players = _context.Players.Where(p => p.RoomId == request.RoomId).Count();

            _context.Players
                .FirstOrDefault(p => p.PlayerId == request.PlayerId && p.RoomId == request.RoomId)
                .Points++;

            if (room.RightGuess + 1 == players - 1)
            {
                room.RightGuess = 0;
                return true;
            }

            room.RightGuess++;

            await _context.SaveChangesAsync(cancellationToken);

            return false;*/
        }
    }
}
