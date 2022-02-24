using MultiplayerSnake.Shared;

namespace MultiplayerSnake.Server
{
    public interface IRoomManager
    {
        IList<Room> GetRooms(int cursor);

        void AddRoom(string id);

        void AddToRoom(string roomId, Player player);
    }

    public class RoomManager : IRoomManager
    {
        public IList<Room> Rooms { get; set; }

        public RoomManager()
        {
            Rooms = new List<Room>();
        }

        public void AddRoom(string id)
        {
            Rooms.Add(new Room
            {
                Id = id
            });
        }

        public void AddToRoom(string roomId, Player player)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == roomId);

            if(room is null)
            {
                // TODO: throw not found error
                return;
            }

            room.Players.Add(player);
        }

        public IList<Room> GetRooms(int page)
        {
            IList<Room> roomsToReturn = Rooms
                .Skip(page * 5)
                .Take(5)
                .ToList();

            return roomsToReturn;
        }
    }
}
