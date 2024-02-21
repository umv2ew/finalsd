using MediatR;
using sd.Jatek.Application.Commands;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class StartGameCommandHandler : IRequestHandler<StartGameCommand, Unit>
    {
        private readonly GameContext _context;
        public StartGameCommandHandler(GameContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(StartGameCommand request, CancellationToken cancellationToken)
        {
            _context.Rooms.FirstOrDefault(r => r.RoomId == request.RoomId).Started = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
