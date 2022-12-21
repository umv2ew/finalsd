namespace sd.Statisztika.Application.Dtos
{
    public class StatisticsDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int PlayedGames { get; set; }
        public int Points { get; set; }
        public int NumberOfWins { get; set; }
    }
}
