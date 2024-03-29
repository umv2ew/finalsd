﻿using MediatR;
using sd.Jatek.Application.Querys;
using sd.Jatek.Infrastructure;

namespace sd.Jatek.Application.QueryHandlers
{
    public class GetNextPlayerQueryHandler : IRequestHandler<GetNextPlayerQuery, bool>
    {
        private readonly GameContext _context;
        public GetNextPlayerQueryHandler(GameContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(GetNextPlayerQuery request, CancellationToken cancellationToken)
        {
            var players = _context.Players
                .Where(p => p.RoomId == request.RoomId)
                .Select(p => p.PlayerId)
                .ToList();

            if (players.Count > 1)
            {
                var painter = _context.Players
                    .FirstOrDefault(p => p.RoomId == request.RoomId && p.PlayerRole == Domain.PlayerRole.Painter);

                var orderByMax = _context.Players.Where(p => p.RoomId == request.RoomId).OrderBy(p => p.Place);

                string nextPainterId = "";
                
                if (painter.Place == orderByMax.Last().Place)
                {
                    nextPainterId = orderByMax.First().PlayerId;
                }
                else
                {
                    nextPainterId = orderByMax.First(pl => pl.Place > painter.Place).PlayerId;
                }

                _context.Players
                    .FirstOrDefault(p => p.RoomId == request.RoomId && p.PlayerId == painter.PlayerId)
                    .PlayerRole = Domain.PlayerRole.Guesser;

                _context.Players
                    .FirstOrDefault(p => p.RoomId == request.RoomId && p.PlayerId == nextPainterId)
                    .PlayerRole = Domain.PlayerRole.Painter;

                var room = _context.Rooms.FirstOrDefault(r => r.RoomId == request.RoomId);

                var roundOver = nextPainterId == orderByMax.First().PlayerId;
                if (roundOver)
                {
                    room.Rounds--;
                }
                room.RightGuess = 0;

                await _context.SaveChangesAsync(cancellationToken);

                return roundOver;
            }
            else
            {
                return true;
            }
        }
    }
}
