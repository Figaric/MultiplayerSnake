using snake;
using System;
using System.Threading;

namespace MultiplayerSnake.Client
{
    class MainMenu
    {
        public int userinput { get; set; }
        public Snake s;
        public Thread upd;
        public Thread kch;
        public bool isAlive = false;
        public bool Logedin = false;
        public MainMenu()
        {
            upd = new Thread(updThread); upd.Start();
            kch = new Thread(keyCheck); kch.Start();
            while (true)
            {
                DrawMainMenu();
            }
        }

        public void DrawMainMenu()
        {
            if (!isAlive)
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
                    switch (userinput = int.Parse(Console.ReadLine()))
                    {
                        case 1:
                            s = new Snake(20);
                            isAlive = true;
                            break;
                        case 2:
                            if (!Logedin)
                            {
                                Console.WriteLine("\n\t\tНеобходимо зарегистрировать аккаунт!");
                                Console.WriteLine("\t\tСделать это можно во вкладке \"Аккаунты\"");
                                Console.WriteLine("\n\t\tНазад - любая клавиша");
                                Console.ReadKey(false);
                            }
                            break;
                        case 3:
                            Console.Clear();
                            if (!Logedin)
                            {
                                Console.WriteLine("\n\t\t1) Зарегистрироваться");
                                Console.WriteLine("\t\t2) Войти");
                                Console.WriteLine("\n\t\tНазад - любая клавиша");
                                switch (Console.ReadKey(false).Key)
                                {
                                    case ConsoleKey.D1:
                                        Console.Clear();
                                        Console.WriteLine("\n\t\t\tReg");
                                        Console.WriteLine("\n\t\tНазад - любая клавиша");
                                        Console.ReadKey(false);
                                        //registration
                                        break;
                                    case ConsoleKey.D2:
                                        Console.Clear();
                                        Console.WriteLine("\n\t\t\tLog-in");
                                        Console.WriteLine("\n\t\tНазад - любая клавиша");
                                        Console.ReadKey(false);
                                        //log-in
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("\n\t\t1) Настройки");
                                Console.WriteLine("\t\t2) Статистика");
                                Console.WriteLine("\n\t\tНазад - любая клавиша");
                                Console.ReadKey(false);
                            }
                            break;
                        case 4:
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
                    Console.WriteLine("Это даже не число, олень ты ёбанный");
                    Console.ReadKey();
                }
            }
        }
        public void updThread()
        {
            while (true)
            {
                if (isAlive)
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
                if (isAlive)
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
                        isAlive = s.IsAlive;
                    }
                }
            }
        }
    }
}
