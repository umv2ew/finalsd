using MediatR;

namespace sd.Jatek.Application.Commands
{
    public class RightGuessCommand : IRequest
    {
        public RightGuessCommand(string roomId, string playerId)
        {
            RoomId = roomId;
            PlayerId = playerId;
        }

        public string RoomId { get; set; }
        public string PlayerId { get; set; }
    }
}
