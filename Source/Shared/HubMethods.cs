using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Shared
{
    public static class HubMethods
    {
        public const string CreateRoom = "CreateRoom";

        public const string JoinRoom = "JoinRoom";

        public const string RoomCreated = "RoomCreated";

        public const string RoomJoined = "RoomJoined";

        public const string GetRooms = "GetRooms";

        public const string RoomsReceived = "RoomsReceived";

        public const string SendPos = "SendPosition";

        public const string PosSent = "PositionSent";
    }
}
