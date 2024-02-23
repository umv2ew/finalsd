using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Querys;
using sd.Jatek.Application.ViewModels;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers;

public class GetWinnerQueryHandler(GameContext context, ILogger<GetWinnerQueryHandler> logger)
    : IRequestHandler<GetWinnerQuery, WinnerViewModel>
{
    private readonly GameContext _context = context;
    private readonly ILogger<GetWinnerQueryHandler> _logger = logger;

    public async Task<WinnerViewModel> Handle(GetWinnerQuery request, CancellationToken cancellationToken)
    {
        var max = await _context.Players
            .Where(p => p.RoomId == request.RoomId)
            .Select(p => p.Points)
            .MaxAsync(cancellationToken);

        var winners = _context.Players
            .Where(p => p.RoomId == request.RoomId && p.Points == max)
            .Select(p => p.PlayerName);

        _logger.LogDebug("The winner in room: {roomId} is {winners} with {points} point", request.RoomId, winners, max);

        return new WinnerViewModel
            (
                winners.Count() > 1,
                max,
                String.Join(", ", [.. winners])
            );
    }
}
