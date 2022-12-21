using MediatR;
using sd.Jatek.Application.Dtos;

namespace sd.Jatek.Application.Querys
{
    public class GetGameDataQuery : IRequest<GameDataDto>
    {
        public GetGameDataQuery(string roomId)
        {
            RoomId = roomId;
        }

        public string RoomId { get; set; }
    }
}
