﻿using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using MultiplayerSnake.Shared;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MultiplayerSnake.Client;

#region JsonTemplates
class Response
{
    public Dictionary<string, string> Data { get; set; }
    public int Statuscode { get; set; }
}
#endregion

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

    public MainMenu()
    {
        upd = new Thread(updThread); upd.Start();
        kch = new Thread(keyCheck); kch.Start();
        SnakeColor = new ColorManager();
        BoundColor = new ColorManager();
        while (true)
        {
            DrawMainMenu();
        }
    }

    public async Task Register(string login, string password)
    {
        var client = new RestClient("http://localhost:5000/account/register/");
        var request = new RestRequest().AddJsonBody(new { Username = login, Password = password });
        var response = await client.PostAsync(request);
    }
    public async Task Login(string login, string password)
    {
        var client = new RestClient("http://localhost:5000/account/login/");
        var request = new RestRequest().AddJsonBody(new { Username = login, Password = password });
        var response = await client.PostAsync(request);
        string jwt = JsonConvert.DeserializeObject<Response>(response.Content).Data["jwtToken"];
        string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\lolsquad\\MultiplayerSnake\\";
        Directory.CreateDirectory(path);
        FileStream f = File.Create(path + "Jwt");
        f.Write(Encoding.UTF8.GetBytes(jwt));
        f.Close();
    }
    public async Task RegisterMenuDraw()
    {
        Console.Clear();
        Console.WriteLine("\n\tВведите логин: ");
        login = Console.ReadLine();
        Console.WriteLine("\n\tВведите пароль: ");
        password = Console.ReadLine();

        Console.Clear();
        Register(login, password);

        Console.WriteLine("\n\t\tYou are succesfully registered");
        Console.WriteLine("\n\t\tНазад - любая клавиша");
        Console.ReadKey(false);
    }
    public async Task LoginMenuDraw()
    {
        Console.Clear();
        Console.WriteLine("\n\tВведите логин: ");
        login = Console.ReadLine();
        Console.WriteLine("\n\tВведите пароль: ");
        password = Console.ReadLine();

        Console.Clear();
        Login(login, password);

        Console.WriteLine("\n\t\tYou are succesfully loged-in");
        Console.WriteLine("\n\t\tНазад - любая клавиша");
        Console.ReadKey(false);
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
            try
            {
                switch (Userinput = int.Parse(Console.ReadLine()))
                {
                    case 1: // Single player
                        s = new Snake(20, SnakeColor, BoundColor);
                        IsAlive = true;
                        break;
                    case 2: // Multiplayer
                        Console.Clear();
                        if (!Logedin)
                        {
                            Console.WriteLine("\n\t\tНеобходимо зарегистрировать аккаунт!");
                            Console.WriteLine("\t\tСделать это можно во вкладке \"Аккаунты\"");
                            Console.WriteLine("\n\t\tНазад - любая клавиша");
                            Console.ReadKey(false);
                        }
                        else // TODO
                        {
                            Console.WriteLine("\n\t\tНеобходимо зарегистрировать аккаунт!");
                            Console.WriteLine("\t\tСделать это можно во вкладке \"Аккаунты\"");
                            Console.WriteLine("\n\t\tНазад - любая клавиша");
                            Console.ReadKey(false);
                        }
                        break;
                    case 3: // Account
                        Console.Clear();
                        if (Logedin)
                        {
                            Console.WriteLine("\n\t\t1) Настройки");
                            Console.WriteLine("\t\t2) Статистика");
                            Console.WriteLine("\n\t\tНазад - любая клавиша");
                            switch (Console.ReadKey(false).Key)
                            {
                                case ConsoleKey.D1:
                                    Console.Clear();
                                    Console.WriteLine("\n\t\t1) Выбор цвета");
                                    Console.WriteLine("\t\t2) Сброс"); //TODO
                                    Console.WriteLine("\n\t\tНазад - любая клавиша");
                                    switch (Console.ReadKey(false).Key)
                                    {
                                        case ConsoleKey.D1:
                                            Console.Clear();
                                            Console.WriteLine("\n\t\t1) Цвет змейки");
                                            Console.WriteLine("\t\t2) Цвет границ");
                                            Console.WriteLine("\n\t\tНазад - любая клавиша");
                                            switch (Console.ReadKey(false).Key)
                                            {
                                                case ConsoleKey.D1:
                                                    Console.Clear();
                                                    Console.WriteLine("\n\t\t1) Белый");
                                                    Console.WriteLine("\t\t2) Красный");
                                                    Console.WriteLine("\t\t3) Зелёный");
                                                    Console.WriteLine("\t\t4) Голубой");
                                                    Console.WriteLine("\t\t5) Маджента");
                                                    Console.WriteLine("\t\t6) Радуга");
                                                    Console.WriteLine("\n\t\tНазад - любая клавиша");
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
                                                    Console.WriteLine("\n\t\t1) Белый");
                                                    Console.WriteLine("\t\t2) Красный");
                                                    Console.WriteLine("\t\t3) Зелёный");
                                                    Console.WriteLine("\t\t4) Голубой");
                                                    Console.WriteLine("\t\t5) Маджента");
                                                    Console.WriteLine("\n\t\tНазад - любая клавиша");
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
                                    Console.WriteLine("\n\t\tИгр сыграно:             stat");
                                    Console.WriteLine("\t\tМаксимальный счёт:       stat");
                                    Console.WriteLine("\t\tВсего съедено:           stat");
                                    Console.WriteLine("\t\tОбщий размер змейки:     stat");
                                    Console.WriteLine("\n\t\tПобед в мултиплеере:     stat");
                                    Console.WriteLine("\t\tПоражений в мултиплеере: stat");
                                    Console.WriteLine("\t\tВинрейт:                 stat");
                                    Console.WriteLine("\n\t\t1) Достижения");
                                    Console.WriteLine("\n\t\tНазад - любая клавиша");
                                    Console.ReadKey(false);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n\t\t1) Зарегистрироваться");
                            Console.WriteLine("\t\t2) Войти");
                            Console.WriteLine("\n\t\tНазад - любая клавиша");
                            switch (Console.ReadKey(false).Key)
                            {
                                case ConsoleKey.D1:
                                    RegisterMenuDraw();
                                    break;
                                case ConsoleKey.D2:
                                    LoginMenuDraw();
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case 4: // Help
                        Console.Clear();
                        Console.WriteLine("\n\t\tУправление -  wasd / стрелочки");
                        Console.WriteLine("\t\tВыход во время игры - Q");
                        Console.WriteLine("\n\t\tНазад - любая клавиша");
                        Console.ReadKey(false);
                        break;
                    default:
                        break;
                }

            }
            catch (System.FormatException)
            {
                Console.WriteLine("Это даже не число, олень ты e#@!ный");
                Console.ReadKey();
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

