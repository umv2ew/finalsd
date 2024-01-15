using MediatR;
using sd.Jatek.Application.Dtos;
using sd.Jatek.Application.Querys;
using sd.Jatek.Application.ViewModels;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers
{
    public class GetGameDataQueryHandler : IRequestHandler<GetGameDataQuery, GameDataViewModel>
    {
        private readonly GameContext _context;

        public GetGameDataQueryHandler(GameContext context)
        {
            _context = context;
        }

        public async Task<GameDataViewModel> Handle(GetGameDataQuery request, CancellationToken cancellationToken)
        {
            var rounds = _context.Rooms.FirstOrDefault(x => x.RoomId == request.RoomId).Rounds;

            var players = _context.Players
                .Where(x => x.RoomId == request.RoomId)
                .Select(x => x.PlayerName)
                .ToList();

            var resultPlayers = String.Join("\n", players);

            return new GameDataViewModel
            {
                Players = resultPlayers,
                playersNumber = players.Count,
                Rounds = rounds,
            };
        }
    }
}
