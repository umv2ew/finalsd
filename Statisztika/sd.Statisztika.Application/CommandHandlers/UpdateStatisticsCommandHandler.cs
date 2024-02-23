using MediatR;
using Microsoft.Extensions.Logging;
using sd.Statisztika.Application.Commands;
using sd.Statisztika.Domain;
using sd.Statisztika.Infrastructure;

namespace sd.Statisztika.Application.CommandHandlers;

public class UpdateStatisticsCommandHandler(StatisticsContext context, ILogger<UpdateStatisticsCommandHandler> logger)
    : IRequestHandler<UpdateStatisticsCommand, Unit>
{
    private readonly StatisticsContext _context = context;
    private readonly ILogger<UpdateStatisticsCommandHandler> _logger = logger;

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

            _logger.LogInformation("Statistics for player {playerId} was created with points: {points}",
                dto.PlayerId,
                dto.Points);
        }
        else
        {
            stat.Points += dto.Points;
            stat.PlayedGames++;
            stat.NumberOfWins += dto.IsWon ? 1 : 0;

            _logger.LogInformation("Statistics for player {playerId} was updated with points: {points}",
                dto.PlayerId,
                dto.Points);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
