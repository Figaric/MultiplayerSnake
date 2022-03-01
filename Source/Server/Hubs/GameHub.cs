using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MultiplayerSnake.Shared;

namespace MultiplayerSnake.Server;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GameHub : Hub
{
    private readonly ILogger<GameHub> _logger;

    private readonly RoomManager _roomManager;

    public GameHub(ILogger<GameHub> logger, RoomManager roomManager)
    {
        _logger = logger;
        _roomManager = roomManager;
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("New connection: " + Context.User.Identity.Name);

        return base.OnConnectedAsync();
    }

    [HubMethodName(HubMethods.CreateRoom)]
    public async Task CreateRoom()
    {
        string roomId = Guid.NewGuid().ToString();

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        _roomManager.AddToRoom(roomId, Context.User.Identity.Name);

        await Clients.Caller.SendAsync(HubMethods.CreateRoom, roomId);
    }

    [HubMethodName(HubMethods.JoinRoom)]
    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        var room = _roomManager.AddToRoom(roomId, Context.User.Identity.Name);

        // Send responses
        // Send group member names to the joined user
        // Send the joined users name to others in the group
        await Clients.Caller.SendAsync(HubMethods.JoinRoom, room.Players.Select(p => p.Nickname));
        await Clients.GroupExcept(roomId, Context.ConnectionId).SendAsync(HubMethods.JoinRoom, new List<string> { Context.User.Identity.Name });
    }

    [HubMethodName(HubMethods.GetRooms)]
    public async Task GetRooms()
    {
        var rooms = _roomManager.GetRooms();

        await Clients.Caller.SendAsync(HubMethods.GetRooms, rooms);
    }

    [HubMethodName(HubMethods.LeaveRoom)]
    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        var room = _roomManager.LeaveRoom(roomId, Context.User.Identity.Name);

        await Clients.Group(roomId).SendAsync(HubMethods.LeaveRoom, room.Players);
    }
}