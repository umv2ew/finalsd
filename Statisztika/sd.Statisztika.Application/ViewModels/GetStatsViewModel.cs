namespace sd.Statisztika.Application.ViewModels
{
    public class GetStatsViewModel
    {
        public int PlayedGames { get; set; }
        public int Points { get; set; }
        public int NumberOfWins { get; set; }
        public string Winrate { get; set; } = default!;
        public string PointPerGame { get; set; } = default!;
    }
}
