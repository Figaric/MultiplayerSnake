using System.Collections.Generic;
using System.Linq;

namespace MultiplayerSnake.Server
{
    public class RoomManager 
    {
        private IList<Room> _rooms;

        public RoomManager()
        {
            _rooms = new List<Room>();
        }

        public Room GetRoomById(string roomId)
        {
            return _rooms.FirstOrDefault(r => r.Id == roomId);
        }

        public bool AddRoom(string ownerId)
        {
            if (GetRoomById(ownerId) != null)
                return false;

            var room = new Room
            {
                Id = ownerId
            };

            _rooms.Add(room);

            return false;
        }
    }
}
