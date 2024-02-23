namespace sd.Jatek.Domain;

public class Room
{
    public Room() { }
    public Room(string id, string roomId, int rounds, int rightGuess, bool started, bool isPublic)
    {
        Id = id;
        RoomId = roomId;
        Rounds = rounds;
        RightGuess = rightGuess;
        Started = started;
        IsPublic = isPublic;
    }

    public string Id { get; set; } = default!;
    public string RoomId { get; set; } = default!;
    public int Rounds { get; set; }
    public int RightGuess { get; set; }
    public bool Started { get; set; }
    public bool IsPublic { get; set; }
}