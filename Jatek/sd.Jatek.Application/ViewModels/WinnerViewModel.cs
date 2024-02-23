namespace sd.Jatek.Application.ViewModels;

public class WinnerViewModel(bool tie, int points, string winners)
{
    public bool Tie { get; set; } = tie;
    public int Points { get; set; } = points;
    public string Winners { get; set; } = winners;
}
