using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MultiplayerSnake.Server
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameHub : Hub
    {
        public async Task SendToMacros(string serverName, string data)
        {
            await Clients.All.SendAsync("MacrosInbound", serverName, data);
        }


        public async Task ConnectorStatus(string serverName, string data)
        {
            await Clients.All.SendAsync("UpdateConnectorStatus", serverName, data);
        }
    }
}
