namespace sd.Jatek.Domain
{
    public class Room
    {
        public Room() { }
        public Room(string id, string roomId, int rounds, int rightGuess, bool started)
        {
            Id = id;
            RoomId = roomId;
            Rounds = rounds;
            RightGuess = rightGuess;
            Started = started;
        }

        public string Id { get; set; }
        public string RoomId { get; set; }
        public int Rounds { get; set; }
        public int RightGuess { get; set; }
        public bool Started { get; set; }
    }
}