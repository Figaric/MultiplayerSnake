using Microsoft.AspNetCore.SignalR.Client;
using MultiplayerSnake.Shared;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerSnake.Client
{
    class NetSession
    {
        public Account UserAccount { get; set; }
        public HubConnection Connection { get; private set; }
        public Room room { get; private set; }

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

                ConfigureListeners();

                await Connection.StartAsync();

                Console.ForegroundColor = ConsoleColor.White;
                
                while (Connection.State != HubConnectionState.Disconnected) //throws exception if server is offline
                {
                    if (Connection.State == HubConnectionState.Connected)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\t\t» Успешно подключен!");
                        Console.ResetColor();
                        Thread.Sleep(500);
                        return;
                    }
                }
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\t» Не удаётся подключится к серверу!");
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

        public void ConfigureListeners()
        {
            Connection.On<IEnumerable<Room>, int>(HubMethods.RoomsReceived, (rooms, page) =>
            {
                Console.WriteLine("\n\tДоступные лобби:\n");
                IList<Room> lrooms = rooms.ToArray();
                for (int i = 0; i < lrooms.Count() - 1; i++)
                {
                    Console.WriteLine($"\t{i + 1}) {lrooms[i].HostName}\t\t{lrooms[i].Id}");

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
                if (lrooms.Count() == 5)
                {
                    Console.Write($"\t\t>>");
                }
                Console.Write($"\n\n\t\tНазад - любая другая клавиша");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        if (lrooms.Count() > 0)
                        {
                            JoinLobbyRequest(lrooms[0].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.D2:
                        if (lrooms.Count() > 1)
                        {
                            JoinLobbyRequest(lrooms[1].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.D3:
                        if (lrooms.Count() > 2)
                        {
                            JoinLobbyRequest(lrooms[2].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.D4:
                        if (lrooms.Count() > 3)
                        {
                            JoinLobbyRequest(lrooms[3].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.D5:
                        if (lrooms.Count() > 4)
                        {
                            JoinLobbyRequest(lrooms[4].Id).GetAwaiter();
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (page > 0)
                        {
                            GetLobbiesRequest(page - 1).GetAwaiter().GetResult();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (lrooms.Count() == 5)
                        {
                            GetLobbiesRequest(page + 1).GetAwaiter().GetResult();
                        }
                        break;
                    default:
                        break;
                }
            });

            Connection.On<string>(HubMethods.RoomCreated, roomId =>
            {
                Console.WriteLine(roomId);
            });
        }

        public void RenderLobbyList(IEnumerable<Room> rooms)
        {
            Console.Clear();
        }
    }
}
