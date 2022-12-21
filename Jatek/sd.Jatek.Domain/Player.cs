namespace sd.Jatek.Domain
{
    public enum PlayerRole
    {
        Guesser,
        Painter
    }

    public class Player
    {
        public Player() { }
        public Player(string id, string roomId, int points, string playerId, string playerName, PlayerRole playerRole)
        {
            Id = id;
            RoomId = roomId;
            Points = points;
            PlayerId = playerId;
            PlayerName = playerName;
            PlayerRole = playerRole;
        }

        public string Id { get; set; }
        public string RoomId { get; set; }
        public int Points { get; set; }
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public PlayerRole PlayerRole { get; set; }
    }
}