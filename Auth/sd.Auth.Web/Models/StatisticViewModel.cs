namespace sd.Auth.Web.Models
{
    public class StatisticViewModel
    {
        public string User { get; set; }
        public int PlayedGames { get; set; }
        public int Points { get; set; }
        public int NumberOfWins { get; set; }
        public decimal Winrate { get; set; }
        public decimal PointPerGame { get; set; }
    }
}
