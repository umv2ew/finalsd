using MediatR;
using sd.Jatek.Application.ViewModels;

namespace sd.Jatek.Application.Querys
{
    public class GetWinnerQuery : IRequest<WinnerViewModel>
    {
        public GetWinnerQuery(string roomId)
        {
            RoomId = roomId;
        }
        public string RoomId { get; set; }
    }
}
