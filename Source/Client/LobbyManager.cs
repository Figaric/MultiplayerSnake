using Microsoft.AspNetCore.SignalR.Client;
using MultiplayerSnake.Shared;
using System;
using System.Linq;

namespace MultiplayerSnake.Client;

public class LobbyManager
{
    public Room Room { get; set; }
    public HubConnection Connection { get; private set; }
    public bool LocalReadyState { get; set; }
    public LobbyManager(Room r, HubConnection hub)
    {
        Room = r;
        Connection = hub;
    }

    public void Draw()
    {
        Console.Clear();
        Console.WriteLine($"\n\tLobby of {Room.Players.FirstOrDefault(h => h.IsHost)}\n");
        for (int i = 0; i < Room.Players.Count - 1; i++)
        {
            Console.Write($"\t\t{i + 1}.\t{Room.Players[i].Nickname}\n\n");
            if (Room.Players[i].IsReady)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Готов\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Не готов\n");
            }
            Console.ResetColor();
        }
        Console.WriteLine("\n\tEsc - покинуть лобби\t\tSpace - готов/не готов");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.Escape:
                Connection.InvokeAsync(HubMethods.LeaveRoom).GetAwaiter();
                break;
            case ConsoleKey.Spacebar:
                LocalReadyState = !LocalReadyState;
                Connection.InvokeAsync(HubMethods.ChangeReadyState, LocalReadyState).GetAwaiter();
                break;
            default:
                Draw();
                break;
        }
    }
}

