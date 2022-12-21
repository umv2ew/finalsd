using MediatR;

namespace sd.Jatek.Application.Querys
{
    public class GetNextPlayerQuery : IRequest<bool>
    {
        public GetNextPlayerQuery(string roomId)
        {
            RoomId = roomId;
        }

        public string RoomId { get; set; }
    }
}
