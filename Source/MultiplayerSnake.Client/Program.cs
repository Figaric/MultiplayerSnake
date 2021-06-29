using System;
using System.Threading;

namespace snake
{
    class Program
    {
        public static Snake s = new Snake(20);
        static void Main(string[] args)
        {
            Thread upd = new Thread(updTread); upd.Start();
            Thread kch = new Thread(keyCheck); kch.Start();
        }

        public static void updTread()
        {
            while (true)
            {
                Thread.Sleep((int)(500/s.Speed));
                s.Update();
            }
        }
        public static void keyCheck()
        {
            while (true)
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
                    System.Environment.Exit(0);
                }
            }
        }
    }
}
