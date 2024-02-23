namespace sd.Jatek.Integration;

public class StatisticsIntegrationDto
{
    public string PlayerId { get; set; } = default!;
    public bool IsWon { get; set; }
    public int Points { get; set; }
}