using sd.Jatek.Application.Models;

namespace sd.Jatek.Application.Dtos
{
    public class RoomDto
    {
        public string RoomId { get; set; } = default!;
        public string PlayerId { get; set; } = default!;
        public string? PlayerName { get; set; }
        public int Rounds { get; set; }
        public bool IsPublic { get; set; }
    }
}
