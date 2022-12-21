using MediatR;

namespace sd.Jatek.Application.Commands
{
    public class AddPointsCommand : IRequest
    {
        public AddPointsCommand(string roomId, string userId)
        {
            RoomId = roomId;
            UserId = userId;
        }
        public string RoomId { get; set; }
        public string UserId { get; set; }
    }
}
