using Shouldly;
using Xunit;

namespace sd.Jatek.Domain.Test
{
    public class Tests
    {
        [Fact]
        public void ShouldCreateRoom()
        {
            var id = "id";
            var roomId = "roomId";
            var rounds = 5;
            var rightGuesses = 2;
            var started = true;
            var publicGame = true;

            var room = new Room(id, roomId, rounds, rightGuesses, started, publicGame);

            room.Id.ShouldBe(id);
            room.RoomId.ShouldBe(roomId);
            room.Rounds.ShouldBe(rounds);
            room.RightGuess.ShouldBe(rightGuesses);
            room.Started.ShouldBe(started);
            room.IsPublic.ShouldBe(publicGame);
        }

        [Fact]
        public void ShouldCreatePlayer()
        {
            var id = "id";
            var roomId = "roomId";
            var points = 5;
            var playerId = "playerId";
            var playerName = "PlayerName";
            var playerRole = PlayerRole.Painter;

            var player = new Player(id, roomId, points, playerId, playerName, playerRole);

            player.Id.ShouldBe(id);
            player.RoomId.ShouldBe(roomId);
            player.Points.ShouldBe(points);
            player.PlayerId.ShouldBe(playerId);
            player.PlayerName.ShouldBe(playerName);
            player.PlayerRole.ShouldBe(playerRole);
        }
    }
}