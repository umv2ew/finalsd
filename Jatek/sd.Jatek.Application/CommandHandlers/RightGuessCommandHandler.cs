using MediatR;
using sd.Jatek.Application.Commands;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class RightGuessCommandHandler : IRequestHandler<RightGuessCommand, Unit>
    {
        private readonly GameContext _context;
        public RightGuessCommandHandler(GameContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(RightGuessCommand request, CancellationToken cancellationToken)
        {
            _context.Players
               .FirstOrDefault(p => p.PlayerId == request.PlayerId && p.RoomId == request.RoomId)
               .Points++;

            _context.Rooms
               .FirstOrDefault(p => p.RoomId == request.RoomId)
               .RightGuess++;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
