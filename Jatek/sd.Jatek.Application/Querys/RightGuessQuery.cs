using MediatR;

namespace sd.Jatek.Application.Querys
{
    public class RightGuessQuery : IRequest<bool>
    {
        public RightGuessQuery(string roomId, string playerId)
        {
            RoomId = roomId;
            PlayerId = playerId;
        }

        public string RoomId { get; set; }
        public string PlayerId { get; set; }
    }
}
