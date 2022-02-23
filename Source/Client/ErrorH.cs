using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Client
{
    enum Errors
    {
        UserAlreadyExists,
        SuchUserDoesNotExist,
        InvalidPassword,
        Success
    }
    class ErrorH
    {
        private Errors currentError;

        public Errors CurrentError { get => currentError; set => currentError = value; }

        public ErrorH()
        {
            currentError = Errors.Success;
        }

        public void Print(bool login)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            switch (CurrentError)
            {
                case Errors.UserAlreadyExists:
                    Console.WriteLine("\n\t\tЭто имя пользователя уже занято!");
                    break;
                case Errors.SuchUserDoesNotExist:
                    Console.WriteLine("\n\t\tНеверное имя пользователя!");
                    break;
                case Errors.InvalidPassword:
                    Console.WriteLine("\n\t\tНеверный пароль!");
                    break;
                case Errors.Success:
                    Console.BackgroundColor = ConsoleColor.Green;
                    if (login)
                    {
                        Console.WriteLine("\n\t\tВы успешно вошли!");
                    }
                    else
                    {
                        Console.WriteLine("\n\t\tВы успешно зарегистрировались!");
                    }
                    break;
                default:
                    break;
            }
            Console.ResetColor();
            Console.WriteLine("\n\t\tДалее - любая клавиша");
        }

        public void MarkField(bool loginField)
        {
            switch (CurrentError)
            {
                case Errors.UserAlreadyExists:
                    if (loginField)
                    {
                        Console.Write("\n\tВведите ");
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("логин");
                        Console.ResetColor();
                        Console.Write(": ");
                    }
                    else
                    {
                        Console.Write("\n\tВведите пароль: ");
                    }
                    break;
                case Errors.SuchUserDoesNotExist:
                    if (loginField)
                    {
                        Console.Write("\n\tВведите ");
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("логин");
                        Console.ResetColor();
                        Console.Write(": ");
                    }
                    else
                    {
                        Console.Write("\n\tВведите пароль: ");
                    }
                    break;
                case Errors.InvalidPassword:
                    if (loginField)
                    {
                        Console.Write("\n\tВведите логин: ");
                    }
                    else
                    {
                        Console.Write("\n\tВведите ");
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("пароль");
                        Console.ResetColor();
                        Console.Write(": ");
                    }
                    break;
                case Errors.Success:
                    if (loginField)
                    {
                        Console.Write("\n\tВведите логин: ");
                    }
                    else
                    {
                        Console.Write("\n\tВведите пароль: ");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
