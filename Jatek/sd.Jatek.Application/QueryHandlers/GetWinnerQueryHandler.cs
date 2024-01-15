using MediatR;
using sd.Jatek.Application.Querys;
using sd.Jatek.Application.ViewModels;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers
{
    public class GetWinnerQueryHandler : IRequestHandler<GetWinnerQuery, WinnerViewModel>
    {
        private readonly GameContext _context;

        public GetWinnerQueryHandler(GameContext context)
        {
            _context = context;
        }

        public async Task<WinnerViewModel> Handle(GetWinnerQuery request, CancellationToken cancellationToken)
        {
            var max = _context.Players
                .Where(p => p.RoomId == request.RoomId)
                .Select(p => p.Points)
                .Max();

            var winners = _context.Players
                .Where(p => p.RoomId == request.RoomId && p.Points == max)
                .Select(p => p.PlayerName);

            return new WinnerViewModel(
                winners.Count() > 1,
                max,
                String.Join(", ", winners.ToArray())
                );
        }
    }
}
