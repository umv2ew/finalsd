using MediatR;

namespace sd.Jatek.Application.Commands
{
    public class RemoveRoomCommand : IRequest
    {
        public RemoveRoomCommand(string roomId)
        {
            RoomId = roomId;
        }
        public string RoomId { get; set; }
    }
}
