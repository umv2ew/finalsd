using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Commands;
using sd.Jatek.Domain;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand>
    {
        private readonly ILogger<CreateRoomCommandHandler> _logger;
        private readonly GameContext _context;
        public CreateRoomCommandHandler(GameContext context, ILogger<CreateRoomCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var alreadyInRoom = await _context.Players
                .FirstOrDefaultAsync(x => x.PlayerId == request.Dto.PlayerId, cancellationToken);

            if (alreadyInRoom != null)
            {
                _context.Players.Remove(alreadyInRoom);

                _logger.LogInformation("Player {player} was removed from the room they were in",
                    alreadyInRoom.PlayerName);
            }

            await _context.Rooms.AddAsync(new Room(
                Guid.NewGuid().ToString(),
                request.Dto.RoomId,
                request.Dto.Rounds,
                0,
                false,
                request.Dto.IsPublic
                ), cancellationToken);

            _logger.LogInformation("New room was created with id {id} and with {rounds} rounds",
                request.Dto.RoomId,
                request.Dto.Rounds);

            await _context.Players.AddAsync(new Player(
                Guid.NewGuid().ToString(),
                request.Dto.RoomId,
                0,
                request.Dto.PlayerId,
                request.Dto.PlayerName ?? "",
                1,
                PlayerRole.Painter), cancellationToken);

            _logger.LogInformation("Player {player} was added to room with id {id} and with {rounds} rounds",
                request.Dto.PlayerName,
                request.Dto.RoomId,
                request.Dto.Rounds);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
