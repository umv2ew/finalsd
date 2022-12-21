using MediatR;
using sd.Jatek.Application.Commands;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class AddPointsCommandHandler : IRequestHandler<AddPointsCommand, Unit>
    {
        private readonly GameContext _context;
        public AddPointsCommandHandler(GameContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(AddPointsCommand request, CancellationToken cancellationToken)
        {
            _context.Players
                .FirstOrDefault(x => x.PlayerId == request.UserId && x.RoomId == request.RoomId)
                .Points++;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
