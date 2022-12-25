using MediatR;

namespace sd.Jatek.Application.Commands
{
    public class RemovePlayerCommand : IRequest
    {
        public RemovePlayerCommand(string playerId)
        {
            PlayerId = playerId;
        }
        public string PlayerId { get; set; }
    }
}
