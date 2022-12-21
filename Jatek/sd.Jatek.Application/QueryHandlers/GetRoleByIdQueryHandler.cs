using MediatR;
using sd.Jatek.Application.Querys;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, string>
    {
        private readonly GameContext _context;
        public GetRoleByIdQueryHandler(GameContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return _context.Players
                .FirstOrDefault(p => p.RoomId == request.RoomId && p.PlayerId == request.PlayerId)
                .PlayerRole
                .ToString();
        }
    }
}
