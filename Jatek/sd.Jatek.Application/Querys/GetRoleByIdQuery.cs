using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sd.Jatek.Application.Querys
{
    public class GetRoleByIdQuery : IRequest<string>
    {
        public GetRoleByIdQuery(string playerId, string roomId)
        {
            PlayerId = playerId;
            RoomId = roomId;
        }
        public string PlayerId { get; set; }
        public string RoomId { get; set; }
    }
}
