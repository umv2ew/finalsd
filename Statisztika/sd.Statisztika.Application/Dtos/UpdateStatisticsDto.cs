namespace sd.Statisztika.Application.Dtos
{
    public class UpdateStatisticsDto
    {
        public UpdateStatisticsDto(string playerId, bool isWon, int points)
        {
            PlayerId = playerId;
            IsWon = isWon;
            Points = points;
        }
        public string PlayerId { get; set; }
        public bool IsWon { get; set; }
        public int Points { get; set; }
    }
}
