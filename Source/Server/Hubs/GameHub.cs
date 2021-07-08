using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> _logger;
        private readonly PlayerManager _playerManager;

        public GameHub(ILogger<GameHub> logger, PlayerManager playerManager)
        {
            _logger = logger;
            _playerManager = playerManager;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("New connection");

            return base.OnConnectedAsync();
        }

        public async void RequestJoinRoom(string roomName)
        {
            if(_playerManager.GetPlayersByCurrentRoom(roomName).Count == 4)
            {
                var response = ResponseBase.Failed(new FieldError
                {
                    Field = nameof(roomName),
                    Message = "The room, you've tried to connect to, is full"
                });

                await Clients.Caller.SendAsync(GameHubResponses.ResponseJoinRoom, response);
                return;
            }

            var addPlayerResponse = _playerManager.AddPlayer(Context.ConnectionId, roomName);

            if (!addPlayerResponse)
            {
                var response = ResponseBase.Failed(new FieldError
                {
                    Field = "",
                    Message = "Something went wrong."
                });

                await Clients.Caller.SendAsync(GameHubResponses.ResponseJoinRoom, response);
                return;
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async void RequestPassPositionInfo(IList<Point> snake)
        {
            var currentPlayer = _playerManager.GetPlayerById(Context.ConnectionId);

            // That's a pretty stOOpid validator xDD, I mean, it just doesn't make any sence to do it
            if(currentPlayer == null)
            {
                var response = ResponseBase.Failed(new FieldError
                {
                    Field = "playerId",
                    Message = "Could not find player with id " + Context.ConnectionId
                });

                await Clients.Caller.SendAsync(GameHubResponses.ResponseJoinRoom, response);
                return;
            }

            var successfulResponse = ResponseBase<IList<Point>>.Succeed(snake);

            await Clients.OthersInGroup(currentPlayer.CurrentRoomName).SendAsync(GameHubResponses.ResponsePassPositionInfo, successfulResponse);
        }
    }
}
