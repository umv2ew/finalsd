using System.ComponentModel.DataAnnotations;

namespace sd.Statisztika.Domain
{
    public class Statistic
    {
        public Statistic() { }
        public Statistic(string id, string userId, int playedGames, int points, int numberOfWins)
        {
            Id = id;
            UserId = userId;
            PlayedGames = playedGames;
            Points = points;
            NumberOfWins = numberOfWins;
        }
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public int PlayedGames { get; set; }
        public int Points { get; set; }
        public int NumberOfWins { get; set; }
    }
}