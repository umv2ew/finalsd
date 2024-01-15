namespace sd.Auth.Web.Models
{
    public class StatisticViewModel
    {
        public string User { get; set; } = default!;
        public int PlayedGames { get; set; }
        public int Points { get; set; }
        public int NumberOfWins { get; set; }
        public string Winrate { get; set; } = default!;
        public string PointPerGame { get; set; } = default!;
    }
}
