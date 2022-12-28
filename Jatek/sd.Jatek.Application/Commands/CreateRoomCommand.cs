using MediatR;
using sd.Jatek.Application.Dtos;

namespace sd.Jatek.Application.Commands
{
    public class CreateRoomCommand : IRequest
    {
        public CreateRoomCommand(RoomDto dto, bool createRoom)
        {
            Dto = dto;
            CreateRoom = createRoom;
        }

        public RoomDto Dto { get; set; }
        public bool CreateRoom { get; set; }
    }
}
