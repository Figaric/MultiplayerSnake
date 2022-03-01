namespace MultiplayerSnake.Shared
{
    public class Room
    {
        public string Id { get; set; }
        public IList<Player> Players { get; set; }
    }
}
