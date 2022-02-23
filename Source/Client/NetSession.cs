using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
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
                Console.Clear();
                Console.WriteLine("\n\tПодключение к серверу...");
                Connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/hubs/gamehub", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(UserAccount.JwtToken);
                    }).Build();

                await Connection.StartAsync();

                while (Connection.State != HubConnectionState.Disconnected)
                {
                    if (Connection.State == HubConnectionState.Connected)
                    {
                        Console.WriteLine("\n\t\tУспешно подключен!");
                        Thread.Sleep(200);
                        return;
                    }
                }
                Console.WriteLine("\n\t\tНе удаётся подключится к серверу!");
                Thread.Sleep(500);
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\n\tВы не вошли в аккаунт");
                Thread.Sleep(500);
            }
        }

        public async Task HostLobby()
        {
            await Connection.InvokeAsync("CreateRoom");
        }
    }
}
