using MediatR;
using sd.Jatek.Application.ViewModels;

namespace sd.Jatek.Application.Querys
{
    public class GetGameDataQuery : IRequest<GameDataViewModel>
    {
        public GetGameDataQuery(string roomId)
        {
            RoomId = roomId;
        }

        public string RoomId { get; set; }
    }
}
