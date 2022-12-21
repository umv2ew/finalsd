using MediatR;

namespace sd.Jatek.Application.Querys
{
    public class GetPainterFinishedQuery : IRequest<string>
    {
        public GetPainterFinishedQuery(string roomId)
        {
            RoomId = roomId;
        }
        public string RoomId { get; set; }
    }
}
