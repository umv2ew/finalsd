using MediatR;
using sd.Jatek.Application.Querys;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers
{
    public class GetPainterFinishedQueryHandler : IRequestHandler<GetPainterFinishedQuery, string>
    {
        private readonly GameContext _context;
        public GetPainterFinishedQueryHandler(GameContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(GetPainterFinishedQuery request, CancellationToken cancellationToken)
        {
            var players = _context.Players.Where(p => p.RoomId == request.RoomId).Count();
            var rigthGuesses = _context.Rooms.FirstOrDefault(r => r.RoomId == request.RoomId).RightGuess;

            if (rigthGuesses == players - 1)
            {
                _context.Rooms.FirstOrDefault(r => r.RoomId == request.RoomId).RightGuess = 0;
                await _context.SaveChangesAsync(cancellationToken);

                return "true";
            }

            return "false";
        }
    }
}
