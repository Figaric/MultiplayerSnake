using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MultiplayerSnake.Shared;

namespace MultiplayerSnake.Server
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> _logger;

        public GameHub(ILogger<GameHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("New connection: " + Context.User.Identity.Name);

            return base.OnConnectedAsync();
        }

        public async Task CreateRoom()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Guid.NewGuid().ToString());
            await Clients.Caller.SendAsync("CreatedRoom", true);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }
    }
}
