using MediatR;
using sd.Statisztika.Application.Commands;
using sd.Statisztika.Domain;
using sd.Statisztika.Infrastructure;

namespace sd.Statisztika.Application.CommandHandlers
{
    public class UpdateStatisticsCommandHandler : IRequestHandler<UpdateStatisticsCommand>
    {
        private readonly StatisticsContext _context;
        public UpdateStatisticsCommandHandler(StatisticsContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(UpdateStatisticsCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var stat = _context.Statistics.FirstOrDefault(x => x.UserId == dto.PlayerId);
            if (stat == null)
            {
                await _context.Statistics.AddAsync(new Statistic(
                    Guid.NewGuid().ToString(),
                    dto.PlayerId,
                    1,
                    dto.Points,
                    dto.IsWon ? 1 : 0
                 ), cancellationToken);
            }
            else
            {
                stat.Points += dto.Points;
                stat.PlayedGames++;
                stat.NumberOfWins += dto.IsWon ? 1 : 0;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
