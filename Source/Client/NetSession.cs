using Microsoft.AspNetCore.SignalR.Client;
using MultiplayerSnake.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace MultiplayerSnake.Client
{
    class NetSession
    {
        public Account UserAccount { get; set; }
        public HubConnection Connection { get; private set; }
        public LobbyManager LobbyManager { get; set; }
        public NetSession(Account userAccount)
        {
            UserAccount = userAccount;
        }

        public async Task Connect()
        {
            if (UserAccount.Logon)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\tПодключение к серверу...");
                Connection = new HubConnectionBuilder()
                    .WithUrl($"{ApiEndpoints.Host}{ApiEndpoints.GameHubRoute}", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(UserAccount.JwtToken);
                    }).Build();

                ConfigureListeners();

                await Connection.StartAsync();

                while (Connection.State != HubConnectionState.Disconnected)
                {
                    if (Connection.State == HubConnectionState.Connected)
                    {
                        Console.Write("\n\t\t» ");
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.WriteLine("Успешно подключен!");
                        Console.ResetColor();
                        Thread.Sleep(500);
                        return;
                    }
                }
                Console.Write("\n\t\t» ");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удаётся подключится к серверу!");
                Console.ResetColor();
                Thread.Sleep(500);
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\n\tВы не вошли в аккаунт");
                Thread.Sleep(1500);
            }
        }

        public async Task HostLobbyRequest()
        {
            await Connection.InvokeAsync(HubMethods.CreateRoom);
        }
        public async Task JoinLobbyRequest(string roomId)
        {
            await Connection.InvokeAsync(HubMethods.JoinRoom, roomId);
        }
        public async Task GetLobbiesRequest(int page = 0)
        {
            await Connection.InvokeAsync(HubMethods.GetRooms, page);
        }
        public async Task ChangeReadyStateRequest(bool state)
        {
            await Connection.InvokeAsync(HubMethods.GetRooms, LobbyManager.Room.Id, state);
        }

        public void ConfigureListeners()
        {
            Connection.On<IList<Player>, string>(HubMethods.JoinRoom, (players, roomId) =>
            {
                if (LobbyManager is null)
                {
                    LobbyManager = new LobbyManager(new Room()
                    {
                        Id = roomId,
                        Players = players
                    }, Connection);
                }
                else
                {
                    LobbyManager.Room.Players.Add(players[0]);
                }
                LobbyManager.Draw();
            });

            Connection.On<IList<Room>, int>(HubMethods.GetRooms, (rooms, page) =>
            {
                Console.WriteLine("\n\tДоступные лобби:\n");
                for (int i = 0; i < rooms.Count() - 1; i++)
                {
                    Console.WriteLine($"\t{i + 1}) {rooms[i].Players.FirstOrDefault(p => p.IsHost).Nickname}\t\t{rooms[i].Players.Count}/5");
                }
                if (page > 0)
                {
                    Console.Write($"\n\t<<\t");
                    Console.Write($"\tСтраница {page}");
                }
                else
                {
                    Console.Write($"\n\tСтраница {page}");
                }
                if (rooms.Count() == 5)
                {
                    Console.Write($"\t\t>>");
                }
                Console.Write($"\n\n\t\tНазад - любая другая клавиша");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        if (rooms.Count() > 0)
                        {
                            JoinLobbyRequest(rooms[0].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.D2:
                        if (rooms.Count() > 1)
                        {
                            JoinLobbyRequest(rooms[1].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.D3:
                        if (rooms.Count() > 2)
                        {
                            JoinLobbyRequest(rooms[2].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.D4:
                        if (rooms.Count() > 3)
                        {
                            JoinLobbyRequest(rooms[3].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.D5:
                        if (rooms.Count() > 4)
                        {
                            JoinLobbyRequest(rooms[4].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (page > 0)
                        {
                            GetLobbiesRequest(page - 1).GetAwaiter().GetResult();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (rooms.Count() == 5)
                        {
                            GetLobbiesRequest(page + 1).GetAwaiter().GetResult();
                        }
                        break;
                    default:
                        break;
                }
            });

            Connection.On<bool, string>(HubMethods.ChangeReadyState, (state, name) =>
            {
                LobbyManager.Room.Players.FirstOrDefault(p => p.Nickname == name).IsReady = state;
                LobbyManager.Draw();
            });

            Connection.On<string>(HubMethods.LeaveRoom, name =>
            {
                if (name is null)
                {
                    LobbyManager = null;
                }
                else
                {
                    var left = LobbyManager.Room.Players.FirstOrDefault(p => p.Nickname == name);
                    LobbyManager.Room.Players.Remove(left);
                    LobbyManager.Draw();
                }
            });

            Connection.On<string>(HubMethods.CreateRoom, roomId =>
            {
                LobbyManager = new LobbyManager(new Room()
                {
                    Id = roomId,
                    Players = new List<Player>() {
                        new Player() {
                            Nickname = UserAccount.Nickname,
                            IsHost = true
                        }
                    }
                }, Connection);
                LobbyManager.Draw();
            });
        }

        public void RenderLobbyList(IEnumerable<Room> rooms)
        {
            Console.Clear();
        }
    }
}
