using MediatR;
using sd.Jatek.Application.Dtos;

namespace sd.Jatek.Application.Querys
{
    public class GetWinnerQuery : IRequest<WinnerDto>
    {
        public GetWinnerQuery(string roomId)
        {
            RoomId = roomId;
        }
        public string RoomId { get; set; }
    }
}
