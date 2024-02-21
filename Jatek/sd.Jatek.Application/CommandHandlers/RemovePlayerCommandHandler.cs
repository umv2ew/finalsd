using MediatR;
using sd.Jatek.Application.Commands;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class RemovePlayerCommandHandler : IRequestHandler<RemovePlayerCommand, Unit>
    {
        private readonly GameContext _context;
        public RemovePlayerCommandHandler(GameContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(RemovePlayerCommand request, CancellationToken cancellationToken)
        {
            var player = _context.Players.FirstOrDefault(p => p.PlayerId == request.PlayerId);

            _context.Players.Remove(player);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
