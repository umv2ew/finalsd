namespace sd.Jatek.Application.Dtos
{
    public class WinnerDto
    {
        public WinnerDto(bool tie, int points, string winners)
        {
            Tie = tie;
            Points = points;
            Winners = winners;
        }
        public bool Tie { get; set; }
        public int Points { get; set; }
        public string Winners { get; set; }
    }
}
