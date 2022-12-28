using MediatR;
using Microsoft.EntityFrameworkCore;
using sd.Statisztika.Application.Queries;
using sd.Statisztika.Application.ViewModels;
using sd.Statisztika.Infrastructure;

namespace sd.Statisztika.Application.QueryHandlers
{
    public class GetStatsQueryHandler : IRequestHandler<GetStatsQuery, GetStatsViewModel>
    {
        private readonly StatisticsContext _context;
        public GetStatsQueryHandler(StatisticsContext context)
        {
            _context = context;
        }
        public async Task<GetStatsViewModel> Handle(GetStatsQuery request, CancellationToken cancellationToken)
        {
            var stat = await _context.Statistics.FirstOrDefaultAsync(x => x.UserId == request.PlayerId, cancellationToken);

            if (stat == null)
            {
                throw new Exception("There is no stat for this id");
            }

            return new GetStatsViewModel
            {
                PlayedGames = stat.PlayedGames,
                Points = stat.Points,
                NumberOfWins = stat.NumberOfWins,
                Winrate = (decimal)stat.NumberOfWins / stat.PlayedGames * 100,
                PointPerGame = (decimal)stat.Points / stat.PlayedGames,
            };
        }
    }
}
