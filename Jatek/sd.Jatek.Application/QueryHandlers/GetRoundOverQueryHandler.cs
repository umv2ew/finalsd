using MediatR;
using sd.Jatek.Application.Querys;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers
{
    public class GetRoundOverQueryHandler : IRequestHandler<GetRoundOverQuery, string>
    {
        private readonly GameContext _context;

        public GetRoundOverQueryHandler(GameContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetRoundOverQuery request, CancellationToken cancellationToken)
        {
            var rounds = _context.Rooms.FirstOrDefault(r => r.RoomId == request.RoomId).Rounds;

            return rounds == 0 ? "true" : "false";
        }
    }
}
