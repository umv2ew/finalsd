using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Querys;
using sd.Jatek.Application.ViewModels;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers
{
    public class GetGameDataQueryHandler : IRequestHandler<GetGameDataQuery, GameDataViewModel>
    {
        private readonly GameContext _context;
        private readonly ILogger<GetGameDataQueryHandler> _logger;

        public GetGameDataQueryHandler(GameContext context, ILogger<GetGameDataQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GameDataViewModel> Handle(GetGameDataQuery request, CancellationToken cancellationToken)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(x => x.RoomId == request.RoomId, cancellationToken);

            if (room == null)
                _logger.LogError("Room with id: {roomId} doesnt exist", request.RoomId);

            var players = await _context.Players
                .Where(x => x.RoomId == request.RoomId)
                .Select(x => x.PlayerName)
                .ToListAsync(cancellationToken);

            var resultPlayers = String.Join("\n", players);

            _logger.LogDebug("{roomId} data: players: {players}, remaining rounds: {rounds}", request.RoomId, players, room?.Rounds);

            return new GameDataViewModel
            {
                Players = resultPlayers,
                playersNumber = players.Count,
                Rounds = room.Rounds,
            };
        }
    }
}
