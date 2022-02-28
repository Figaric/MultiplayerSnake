namespace MultiplayerSnake.Shared
{
    public class Room
    {
        public string Id { get; set; }

        public string HostName { get; set; }

        public IEnumerable<string> UserNames { get; set; } 
    }
}
