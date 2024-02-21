using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Querys;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, string>
    {
        private readonly GameContext _context;
        private readonly ILogger<GetRoleByIdQueryHandler> _logger;

        public GetRoleByIdQueryHandler(GameContext context, ILogger<GetRoleByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<string> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.RoomId == request.RoomId && p.PlayerId == request.PlayerId, cancellationToken);

            _logger.LogDebug("Players {playerId} role is {playerRole}", request.PlayerId, player?.PlayerRole);

            return player == null ? "" : player.PlayerRole.ToString();
        }
    }
}
