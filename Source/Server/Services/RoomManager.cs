using System.Collections.Generic;
using MultiplayerSnake.Shared;

namespace MultiplayerSnake.Server;

public class RoomManager
{
    private IList<Room> _rooms;

    public RoomManager()
    {
        _rooms = new List<Room>();
    }

    public IList<Room> GetRooms()
    {
        return _rooms;
    }

    public Room AddToRoom(string roomId, string userName, bool isHost = false)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == roomId);

        var player = new Player 
        {
            Nickname = userName, 
            IsHost = isHost 
        };

        if(room != null)
        {
            room.Players.Add(player);
            return room;
        }

        room = new Room
        {
            Id = roomId,
            Players = new List<Player> { player }
        };

        _rooms.Add(room);

        return room;
    }

    public Room LeaveRoom(string roomId, string userName)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == roomId);

        room.Players.ToList()
            .RemoveAll(p => p.Nickname == userName);

        return room;
    }
}