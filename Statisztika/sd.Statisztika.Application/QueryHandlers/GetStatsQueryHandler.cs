using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Statisztika.Application.Queries;
using sd.Statisztika.Application.ViewModels;
using sd.Statisztika.Infrastructure;

namespace sd.Statisztika.Application.QueryHandlers
{
    public class GetStatsQueryHandler : IRequestHandler<GetStatsQuery, GetStatsViewModel>
    {
        private readonly StatisticsContext _context;
        private readonly ILogger<GetStatsQueryHandler> _logger;

        public GetStatsQueryHandler(StatisticsContext context, ILogger<GetStatsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GetStatsViewModel> Handle(GetStatsQuery request, CancellationToken cancellationToken)
        {
            var stat = await _context.Statistics
                .FirstOrDefaultAsync(x => x.UserId == request.PlayerId, cancellationToken);

            if (stat == null)
                throw new Exception("There is no stat for this id");

            var stats = new GetStatsViewModel
            {
                PlayedGames = stat.PlayedGames,
                Points = stat.Points,
                NumberOfWins = stat.NumberOfWins,
                Winrate = ((decimal)stat.NumberOfWins / stat.PlayedGames * 100).ToString("0.####"),
                PointPerGame = ((decimal)stat.Points / stat.PlayedGames).ToString("0.####"),
            };

            _logger.LogDebug("Sending player: {playerId} statistics: {@stats}", request.PlayerId, stats);

            return stats;
        }
    }
}
