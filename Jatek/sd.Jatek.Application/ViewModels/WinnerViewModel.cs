namespace sd.Jatek.Application.ViewModels
{
    public class WinnerViewModel
    {
        public WinnerViewModel(bool tie, int points, string winners)
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
