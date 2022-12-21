using MediatR;

namespace sd.Jatek.Application.Commands
{
    public class EnterRoomCommand : IRequest<bool>
    {
        public EnterRoomCommand(string roomId, string playerId, string playerName)
        {
            RoomId = roomId;
            PlayerId = playerId;
            PlayerName = playerName;
        }

        public string RoomId { get; set; }
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}
