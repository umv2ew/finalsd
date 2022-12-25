namespace sd.Statisztika.Application.ViewModels
{
    public class GetStatsViewModel
    {
        public int PlayedGames { get; set; }
        public int Points { get; set; }
        public int NumberOfWins { get; set; }
        public decimal Winrate { get; set; }
        public decimal PointPerGame { get; set; }
    }
}
