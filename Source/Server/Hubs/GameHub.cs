using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiplayerSnake.Server.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server
{
    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> _logger;
        private readonly JwtManager _jwt;
        private readonly PlayerManager _playerManager;
        private readonly RoomManager _roomManager;
        private readonly ApplicationDbContext _context;

        public GameHub(ILogger<GameHub> logger, JwtManager jwt, PlayerManager playerManager, ApplicationDbContext context, RoomManager roomManager)
        {
            _logger = logger;
            _jwt = jwt;
            _playerManager = playerManager;
            _context = context;
            _roomManager = roomManager;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("New connection!");

            return base.OnConnectedAsync();
        }

        public async void RequestGoOnline(string accessToken)
        {
            var userId = _jwt.GetPayload(accessToken);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var player = new Player
            {
                Id = Context.ConnectionId,
                Color = (ConsoleColor)user.Color,
                UserId = userId
            };

            _playerManager.AddPlayer(player);
        }

        public async void RequestHostRoom()
        {
            if(_roomManager.GetRoomById(Context.ConnectionId) != null)
            {
                var response = ResponseBase.Failed(new FieldError
                {
                    Field = "",
                    Message = "You're already owning a room"
                });

                await Clients.Client(Context.ConnectionId).SendAsync("response-host-lobby", response);

                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, Context.ConnectionId);
            _roomManager.AddRoom(Context.ConnectionId);
        }

        public async void RequestJoinRoom(string roomId)
        {
            var room = _roomManager.GetRoomById(roomId);

            if(room == null)
            {
                var response = ResponseBase.Failed(new FieldError
                {
                    Field = "roomId",
                    Message = "There is no such room with id " + roomId
                });

                await Clients.Client(Context.ConnectionId).SendAsync("response-join-room", response);
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async void RequestSendData(IList<Point> points)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("request-get-data");
        }
    }
}
