using MediatR;
using sd.Jatek.Application.Dtos;

namespace sd.Jatek.Application.Commands;

public class CreateRoomCommand(RoomDto dto, bool createRoom) : IRequest<Unit>
{
    public RoomDto Dto { get; set; } = dto;
    public bool CreateRoom { get; set; } = createRoom;
}
