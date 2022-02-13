using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MultiplayerSnake.Server
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("New connection: " + Context.User.Identity.Name);

            return base.OnConnectedAsync();
        }

        public async Task CreateRoom()
        {
            string roomId = Guid.NewGuid().ToString();

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Caller.SendAsync("CreateRoomResponse", roomId);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.GroupExcept(roomId, Context.ConnectionId).SendAsync("NewPlayer", Context.User.Identity.Name);
        }
    }
}
