using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Client
{
    class MainMenu
    {
        public string userinput { get; set; }
        public MainMenu()
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
            Console.WriteLine("\t4) ВЗЯТЬ КРЕДИТ В АЛЬФАБАНКЕ ПОД 9.9% ГОДОВЫХ");
            Console.ResetColor();
            Console.Write("\nВыберите одну из опций выше: ");
            userinput = Console.ReadLine();

        }
    }
}
