using Shouldly;
using Xunit;

namespace sd.Statisztika.Domain.Test
{
    public class Tests
    {

        [Fact]
        public void ShouldCreatStatistic()
        {
            var id = "id";
            var userId = "userId";
            var playedGames = 3;
            var points = 5;
            var numberOfWins = 2;

            var statistic = new Statistic(id, userId, playedGames, points, numberOfWins);

            statistic.Id.ShouldBe(id);
            statistic.UserId.ShouldBe(id);
            statistic.PlayedGames.ShouldBe(playedGames);
            statistic.Points.ShouldBe(points);
            statistic.NumberOfWins.ShouldBe(numberOfWins);
        }
    }
}