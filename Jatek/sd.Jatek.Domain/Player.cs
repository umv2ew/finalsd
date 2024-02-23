namespace sd.Jatek.Domain;

public enum PlayerRole
{
    Guesser,
    Painter
}

public class Player
{
    public Player() { }
    public Player(string id, string roomId, int points, string playerId, string playerName, int place, PlayerRole playerRole)
    {
        Id = id;
        RoomId = roomId;
        Points = points;
        PlayerId = playerId;
        PlayerName = playerName;
        Place = place;
        PlayerRole = playerRole;
    }

    public string Id { get; set; } = default!;
    public string RoomId { get; set; } = default!;
    public int Points { get; set; }
    public string PlayerId { get; set; } = default!;
    public string PlayerName { get; set; } = default!;
    public int Place { get; set; }
    public PlayerRole PlayerRole { get; set; }
}