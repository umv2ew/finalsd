namespace sd.Statisztika.Application.Dtos;

public class UpdateStatisticsDto(string playerId, bool isWon, int points)
{
    public string PlayerId { get; set; } = playerId;
    public bool IsWon { get; set; } = isWon;
    public int Points { get; set; } = points;
}
