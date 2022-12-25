using MediatR;

namespace sd.Jatek.Application.Commands
{
    public class StartGameCommand : IRequest
    {
        public StartGameCommand(string roomId)
        {
            RoomId = roomId;
        }
        public string RoomId { get; set; }
    }
}
