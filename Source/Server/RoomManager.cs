using MultiplayerSnake.Shared;

namespace MultiplayerSnake.Server
{
    public class RoomManager
    {
        public IList<Room> Rooms { get; set; }

        public RoomManager()
        {
            Rooms = new List<Room>();
        }

        public async void CreateRoom(string roomId, string hostName)
        {
            Room room = new Room
            {
                Id = roomId,
                HostName = hostName,
            };
        }

        public IEnumerable<Room> GetRooms(int page)
        {
            return Rooms.ToList()
                .Skip(5 * page)
                .Take(5);
        }
    }
}
