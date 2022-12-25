namespace sd.Auth.Web.Models
{
    public class StatisticsViewModel
    {
        public string PlayerName { get; set; } = default!;
        public int PlayedGames { get; set; }
        public int Points { get; set; }
        public int NumberOfWins { get; set; }
    }
}
