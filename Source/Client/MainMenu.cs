using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using MultiplayerSnake.Shared;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace MultiplayerSnake.Client;

class MainMenu
{
    public int Userinput { get; set; }
    public Snake s;
    private Thread upd;
    private Thread kch;
    public bool IsAlive;
    public bool Logedin = false;
    public ColorManager SnakeColor;
    public ColorManager BoundColor;
    private string login, password;
    public Account UserAccount;

    public MainMenu()
    {
        upd = new Thread(updThread); upd.Start();
        kch = new Thread(keyCheck); kch.Start();
        SnakeColor = new ColorManager();
        BoundColor = new ColorManager();
        UserAccount = new Account();
        while (true)
        {
            _ = DrawMainMenu();
        }
    }

    public async Task DrawMainMenu()
    {
        if (!IsAlive)
        {
            Console.Clear();
            Console.WriteLine("\n\t\tWelcome to the");
            Console.WriteLine("\t  _____             _        \n\t / ____|           | |       \n\t| (___  _ __   __ _| | _____ \n\t \\___ \\| '_ \\ / _` | |/ / _ \\\n\t ____) | | | | (_| |   <  __/\n\t|_____/|_| |_|\\__,_|_|\\_\\___|");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\t1) Одиночная игра");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t2) Многопользовательская игра");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t3) Аккаунты (Under Constraction)");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\t4) Помощь");
            Console.ResetColor();
            Console.Write("\nВыберите одну из опций выше: ");

            switch (Console.ReadKey(false).Key)
            {
                case ConsoleKey.D1: // Single player
                    s = new Snake(20, SnakeColor, BoundColor);
                    IsAlive = true;
                    break;
                case ConsoleKey.D2: // Multiplayer
                    Console.Clear();
                    if (UserAccount.Logon)
                    {
                        Console.WriteLine("\n\t\t1) Создать\n\t\t1) Присоедениться\n\n\t\tНазад - любая клавиша");
                        switch (Console.ReadKey(false).Key)
                        {
                            case ConsoleKey.D1:
                                Console.Clear();
                                // lobby create request
                                break;
                            case ConsoleKey.D2:
                                Console.Clear();
                                Console.Write("\n\t\tВведите id лобби: ");
                                string id = Console.ReadLine();
                                // connect to lobby request
                                break;
                            default:
                                break;
                        }
                        
                    }
                    else 
                    {
                        Console.WriteLine("\n\t\tНеобходимо зарегистрироваться или войти в аккаунт!\n\t\tСделать это можно во вкладке \"Аккаунты\"\n\n\t\tНазад - любая клавиша");
                        Console.ReadKey(false);
                    }
                    break;
                case ConsoleKey.D3: // Account
                    Console.Clear();
                    if (UserAccount.Logon)
                    {
                        Console.WriteLine("\n\t\t1) Настройки\n\t\t2) Статистика\n\n\t\tНазад - любая клавиша");
                        switch (Console.ReadKey(false).Key)
                        {
                            case ConsoleKey.D1:
                                Console.Clear();
                                Console.WriteLine("\n\t\t1) Выбор цвета\n\t\t2) Сброс\n\n\t\tНазад - любая клавиша"); // TODO clear data
                                switch (Console.ReadKey(false).Key)
                                {
                                    case ConsoleKey.D1:
                                        Console.Clear();
                                        Console.WriteLine("\n\t\t1) Цвет змейки\n\t\t2) Цвет границ\n\n\t\tНазад - любая клавиша");
                                        switch (Console.ReadKey(false).Key)
                                        {
                                            case ConsoleKey.D1:
                                                Console.Clear();
                                                Console.WriteLine("\n\t\t1) Белый\n\t\t2) Красный\n\t\t3) Зелёный\n\t\t4) Голубой\n\t\t5) Маджента\n\t\t6) Радуга\n\n\t\tНазад - любая клавиша");
                                                switch (Console.ReadKey(false).Key)
                                                {
                                                    case ConsoleKey.D1:
                                                        SnakeColor.SetColor(ConsoleColor.White);
                                                        break;
                                                    case ConsoleKey.D2:
                                                        SnakeColor.SetColor(ConsoleColor.Red);
                                                        break;
                                                    case ConsoleKey.D3:
                                                        SnakeColor.SetColor(ConsoleColor.Green);
                                                        break;
                                                    case ConsoleKey.D4:
                                                        SnakeColor.SetColor(ConsoleColor.Cyan);
                                                        break;
                                                    case ConsoleKey.D5:
                                                        SnakeColor.SetColor(ConsoleColor.Magenta);
                                                        break;
                                                    case ConsoleKey.D6:
                                                        SnakeColor.SetColor(ConsoleColor.Black);
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                break;
                                            case ConsoleKey.D2:
                                                Console.Clear();
                                                Console.WriteLine("\n\t\t1) Белый\n\t\t2) Красный\n\t\t3) Зелёный\n\t\t4) Голубой\n\t\t5) Маджента\n\n\t\tНазад - любая клавиша");
                                                switch (Console.ReadKey(false).Key)
                                                {
                                                    case ConsoleKey.D1:
                                                        BoundColor.SetColor(ConsoleColor.White);
                                                        break;
                                                    case ConsoleKey.D2:
                                                        BoundColor.SetColor(ConsoleColor.Red);
                                                        break;
                                                    case ConsoleKey.D3:
                                                        BoundColor.SetColor(ConsoleColor.Green);
                                                        break;
                                                    case ConsoleKey.D4:
                                                        BoundColor.SetColor(ConsoleColor.Cyan);
                                                        break;
                                                    case ConsoleKey.D5:
                                                        BoundColor.SetColor(ConsoleColor.Magenta);
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case ConsoleKey.D2:
                                Console.Clear();
                                Console.WriteLine("\n\t\tИгр сыграно:             stat" +
                                    "\n\t\tМаксимальный счёт:       stat" +
                                    "\n\t\tВсего съедено:           stat" +
                                    "\n\t\tОбщий размер змейки:     stat" +
                                    "\n\n\t\tПобед в мултиплеере:     stat" +
                                    "\n\t\tПоражений в мултиплеере: stat" +
                                    "\n\t\tВинрейт:                 stat" +
                                    "\n\n\t\t1) Достижения" +
                                    "\n\n\t\tНазад - любая клавиша");
                                Console.ReadKey(false);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n\t\t1) Зарегистрироваться\n\t\t2) Войти\n\n\t\tНазад - любая клавиша");
                        switch (Console.ReadKey(false).Key)
                        {
                            case ConsoleKey.D1:
                                Console.Clear();
                                Console.Write("\n\tВведите логин: ");
                                login = Console.ReadLine();
                                Console.Write("\n\tВведите пароль: ");
                                password = Console.ReadLine();

                                Console.Clear();
                                await UserAccount.Register(login, password);
                                Thread.Sleep(100);
                                Console.WriteLine("\n\t\tНазад - любая клавиша");
                                Console.ReadKey(false);
                                break;
                            case ConsoleKey.D2:
                                Console.Clear();
                                Console.Write("\n\tВведите логин: ");
                                login = Console.ReadLine();
                                Console.Write("\n\tВведите пароль: ");
                                password = Console.ReadLine();

                                Console.Clear();
                                await UserAccount.Login(login, password);
                                Thread.Sleep(100);
                                Console.WriteLine("\n\t\tНазад - любая клавиша");
                                Console.ReadKey(false);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case ConsoleKey.D4: // Help
                    Console.Clear();
                    Console.WriteLine("\n\t\tУправление -  wasd / стрелочки\n\t\tВыход во время игры - Q\n\n\t\tНазад - любая клавиша");
                    Console.ReadKey(false);
                    break;
                case ConsoleKey.Q: // Quit
                    Console.WriteLine("Shutting down...");
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
    public void updThread()
    {
        while (true)
        {
            if (IsAlive)
            {
                Thread.Sleep((int)(500 / s.Speed));
                s.Update();
            }
        }
    }
    public void keyCheck()
    {
        while (true)
        {
            if (IsAlive)
            {
                ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.A && s.Direction != 1 || keyInfo.Key == ConsoleKey.LeftArrow && s.Direction != 1)
                {
                    s.Direction = 3;
                }
                else if (keyInfo.Key == ConsoleKey.D && s.Direction != 3 || keyInfo.Key == ConsoleKey.RightArrow && s.Direction != 3)
                {
                    s.Direction = 1;
                }
                else if (keyInfo.Key == ConsoleKey.W && s.Direction != 0 || keyInfo.Key == ConsoleKey.UpArrow && s.Direction != 0)
                {
                    s.Direction = 2;
                }
                else if (keyInfo.Key == ConsoleKey.S && s.Direction != 2 || keyInfo.Key == ConsoleKey.DownArrow && s.Direction != 2)
                {
                    s.Direction = 0;
                }
                else if (keyInfo.Key == ConsoleKey.Q)
                {
                    s.GameOver();
                    IsAlive = s.IsAlive;
                }
            }
        }
    }
}

