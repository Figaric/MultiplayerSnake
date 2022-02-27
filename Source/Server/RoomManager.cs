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

        public Room GetRoom(string roomId)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == roomId);

            if (room == null)
                throw new NullReferenceException("Room not found");

            return room;
        }

        public void CreateRoom(string roomId, string hostName)
        {
            Room room = new Room
            {
                Id = roomId,
                HostName = hostName,
            };
        }

        public void AddToRoom(string roomId, string username)
        {
            var room = GetRoom(roomId);

            room.UserNames.ToList()
                .Add(username);
        }

        public IEnumerable<Room> GetRooms(int page)
        {
            return Rooms.ToList()
                .Skip(5 * page)
                .Take(5);
        }
    }
}
