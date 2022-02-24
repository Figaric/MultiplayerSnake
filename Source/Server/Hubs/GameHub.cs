﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MultiplayerSnake.Shared;

namespace MultiplayerSnake.Server
{
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
            _roomManager.CreateRoom(roomId, Context.User.Identity.Name);

            await Clients.Caller.SendAsync(HubMethods.RoomCreated, roomId);
        }

        [HubMethodName(HubMethods.JoinRoom)]
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.GroupExcept(roomId, Context.ConnectionId).SendAsync(HubMethods.RoomJoined, Context.User.Identity.Name);
        }

        [HubMethodName(HubMethods.GetRooms)]
        public async Task GetRooms(int page)
        {
            var rooms = _roomManager.GetRooms(page);

            await Clients.Caller.SendAsync(HubMethods.RoomsReceived, rooms, page);
        }

        [HubMethodName(HubMethods.SendPos)]
        public async Task SendPos(string roomId, SnakeBase pos)
        {
            await Clients.GroupExcept(roomId, Context.ConnectionId).SendAsync(HubMethods.PosSent, pos);
        }
    }
}
