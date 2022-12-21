using MediatR;

namespace sd.Jatek.Application.Querys
{
    public class GetRoundOverQuery : IRequest<string>
    {
        public GetRoundOverQuery(string roomId)
        {
            RoomId = roomId;
        }
        public string RoomId { get; set; }
    }
}
