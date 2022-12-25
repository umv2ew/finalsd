using MediatR;
using Microsoft.EntityFrameworkCore;
using sd.Jatek.Application.Commands;
using sd.Jatek.Domain;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.CommandHandlers
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, string>
    {
        private readonly GameContext _context;
        public CreateRoomCommandHandler(GameContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var alreadyInRoom = await _context.Players
                .FirstOrDefaultAsync(x => x.PlayerId == request.Dto.PlayerId, cancellationToken);

            if (alreadyInRoom == null)
            {
                await _context.Rooms.AddAsync(new Room(
                     Guid.NewGuid().ToString(),
                    request.Dto.RoomId,
                    request.Dto.Rounds,
                    0,
                    false,
                    request.Dto.IsPublic
                    ), cancellationToken);

                await _context.Players.AddAsync(new Player(
                    Guid.NewGuid().ToString(),
                    request.Dto.RoomId,
                    0,
                    request.Dto.PlayerId,
                    request.Dto.PlayerName,
                    PlayerRole.Painter), cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return "";
            }
            else
            {
                return alreadyInRoom.RoomId;
            }
        }
    }
}
