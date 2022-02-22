using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Client
{
    class NetSession
    {
        public Account UserAccount { get; set; }
        public HubConnection Connection { get; private set; }

        public NetSession(Account userAccount)
        {
            UserAccount = userAccount;
        }

        public async Task Connect()
        {
            if (UserAccount.Logon)
            {
                Connection = new HubConnectionBuilder()
                    .WithUrl($"http://localhost:5000/{87}", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(UserAccount.JwtToken);
                    }).Build();

                await Connection.StartAsync();
            }
        }
    }
}
