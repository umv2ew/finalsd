using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Querys;
using sd.Jatek.Application.ViewModels;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers;

public class GetPublicRoomsQueryHandler(GameContext context, ILogger<GetPublicRoomsQueryHandler> logger)
    : IRequestHandler<GetPublicRoomsQuery, List<PublicRoomsViewModel>>
{
    private readonly GameContext _context = context;
    private readonly ILogger<GetPublicRoomsQueryHandler> _logger = logger;

    public async Task<List<PublicRoomsViewModel>> Handle(GetPublicRoomsQuery request, CancellationToken cancellationToken)
    {
        var roomIds = _context.Rooms.Where(r => r.IsPublic && !r.Started).Select(r => r.RoomId);

        _logger.LogDebug("public rooms: {rooms}", roomIds);

        return await _context.Players
            .Where(p => p.PlayerRole == Domain.PlayerRole.Painter && roomIds.Contains(p.RoomId))
            .Select(p => new PublicRoomsViewModel
            {
                Creator = p.PlayerName,
                RoomId = p.RoomId,
            }).ToListAsync(cancellationToken);
    }
}
