using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Client
{
    class MainMenu
    {
        public int userinput { get; set; }
        public MainMenu()
        {
            while (userinput != 1)
            {
                DrawMainMenu();
            }
        }

        public void DrawMainMenu()
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
                userinput = int.Parse(Console.ReadLine());
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Это даже не число, олень ты ёбанный");
                Console.ReadKey();
            }
            switch (userinput)
            {
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
    }
}
