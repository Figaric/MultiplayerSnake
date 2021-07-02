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
        public MainMenu()
        {
            upd = new Thread(updTread); upd.Start();
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
                            if (true)
                            {

                            }
                            s = new Snake(20);
                            isAlive = true;
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("\n\t\tУправление -  wasd / стрелочки");
                            Console.WriteLine("\t\tВыход во время игры - Q");
                            Console.WriteLine("\n\t\tНазад - любая клавиша");
                            Console.ReadKey();
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
        public void updTread()
        {
            while (true)
            {
                if (isAlive)
                {
                    Thread.Sleep((int)(500 / s.Speed));
                    s.Update();
                    isAlive = s.IsAlive;
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
                    if (keyInfo.Key == ConsoleKey.D && s.Direction != 3 || keyInfo.Key == ConsoleKey.RightArrow && s.Direction != 3)
                    {
                        s.Direction = 1;
                    }
                    if (keyInfo.Key == ConsoleKey.W && s.Direction != 0 || keyInfo.Key == ConsoleKey.UpArrow && s.Direction != 0)
                    {
                        s.Direction = 2;
                    }
                    if (keyInfo.Key == ConsoleKey.S && s.Direction != 2 || keyInfo.Key == ConsoleKey.DownArrow && s.Direction != 2)
                    {
                        s.Direction = 0;
                    }
                    if (keyInfo.Key == ConsoleKey.Q)
                    {
                        s.IsAlive = false;
                        isAlive = false;
                        DrawMainMenu();
                    }
                }
            }
        }
    }
}
