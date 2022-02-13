using MultiplayerSnake.Shared;
using System;
using System.Collections.Generic;

namespace MultiplayerSnake.Client;

class Snake
{
    #region Properties
    public int Direction { get; set; }
    private List<Point> SParts { get; set; }
    private char[,] Map { get; set; }
    private List<Point> FoodCoords { get; set; }
    public double Speed { get; set; }
    private int MapSize { get; set; }
    private int Score { get; set; }
    private int IterCount { get; set; }
    private Random Rnd { get; set; }
    public bool IsAlive { get; set; }
    public ColorManager Color;
    #endregion

    #region Constructor
    public Snake(int x, ColorManager c)
    {
        Direction = 0;
        Map = new char[x, x];
        MapSize = (int)Math.Sqrt(Map.Length) - 1;
        for (int i = 0; i < MapSize; i++) // Generating map bounds
        {
            Map[0, i] = '#';
            Map[i, 0] = '#';
            Map[MapSize, i] = '#';
            Map[i, MapSize] = '#';
        }
        Map[MapSize, MapSize] = '#';
        SParts = new List<Point>() { new Point((MapSize + 1) / 2, (MapSize + 1) / 2) };
        FoodCoords = new List<Point>() { };
        Score = 0;
        Speed = 1;
        Rnd = new Random();
        IsAlive = true;
        Color = c;
    }
    #endregion

    #region Funcs
    public void Draw()
    {
        Console.Clear();
        for (int i = 0; i < MapSize + 1; i++) //  Ingame ui
        {
            for (int j = 0; j < MapSize + 1; j++)
            {
                if (Map[i, j] == '*')
                {
                    Console.ForegroundColor = Color.ReturnColor();
                    Console.Write(Map[i, j]);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(Map[i, j]);
                }
            }
            Console.Write("\n");
        }
        Console.WriteLine($"Score: {Score}");
    }
    public void Update()
    {
        if (IsAlive)
        {
            Point lCoords = SParts[0]; // Saves coords of the first snake's chunk
            switch (Direction) // Moving snake's chunks     PS. Idk what the fuck i just did but it works, so DON'T TOUCH
            {
                case 0: // Up
                    SParts.Remove(SParts[SParts.Count - 1]);
                    SParts.Reverse();
                    SParts.Add(new Point(lCoords.X + 1, lCoords.Y));
                    SParts.Reverse();
                    break;
                case 1: // Right
                    SParts.Remove(SParts[SParts.Count - 1]);
                    SParts.Reverse();
                    SParts.Add(new Point(lCoords.X, lCoords.Y + 1));
                    SParts.Reverse();
                    break;
                case 2: // Down
                    SParts.Remove(SParts[SParts.Count - 1]);
                    SParts.Reverse();
                    SParts.Add(new Point(lCoords.X - 1, lCoords.Y));
                    SParts.Reverse();
                    break;
                case 3: // Left
                    SParts.Remove(SParts[SParts.Count - 1]);
                    SParts.Reverse();
                    SParts.Add(new Point(lCoords.X, lCoords.Y - 1));
                    SParts.Reverse();
                    break;
                default:
                    break;
            }
            MapConstruct();
            IterCount++;
            if (IterCount % MapSize * Speed == 0 && Speed < 50) // Speeding up the game
            {
                Speed = Math.Round(Speed * 1.1, 2);
            }
            GenerateFood();
            Draw();
            if (SParts[0].X == MapSize || SParts[0].Y == MapSize || SParts[0].X == 0 || SParts[0].Y == 0) // Checking if the player is out of bounds
            {
                GameOver();
            }
            SelfCollideCheck();
        }
    }
    private void GenerateFood()
    {
        if (IterCount % MapSize == 0 || FoodCoords.Count == 0) // If it's time to spawn food or there is no food on the map
        {
            FoodCoords.Add(new Point(Rnd.Next(1, MapSize), Rnd.Next(1, MapSize)));
        }
    }
    private void MapConstruct()
    {
        for (int i = 1; i < MapSize; i++)
        {
            for (int j = 1; j < MapSize; j++)
            {
                Map[i, j] = ' '; // Everything exept bounds is void now
                foreach (var part in SParts) // Map construction based on the position of the snake parts
                {
                    foreach (var food in FoodCoords)
                    {
                        if (i == food.X && j == food.Y)
                        {
                            Map[i, j] = '+';
                        }
                        if (food.X == part.X && food.Y == part.Y) // If snake eats food
                        {
                            FoodCoords.Remove(food);
                            Map[food.X, food.Y] = '*'; // Render snake's chunk at food coords
                            SnakeGrowth();
                            Score++;
                            return;
                        }
                    }
                    if (i == part.X && j == part.Y) // Render snake chunks
                    {
                        Map[i, j] = '*';
                    }
                }
            }
        }
    }
    private void SnakeGrowth()
    {
        switch (Direction)
        {
            case 0:
                SParts.Add(new Point(SParts[0].X + 1, SParts[0].Y));
                break;
            case 1:
                SParts.Add(new Point(SParts[0].X, SParts[0].Y + 1));
                break;
            case 2:
                SParts.Add(new Point(SParts[0].X - 1, SParts[0].Y));
                break;
            case 3:
                SParts.Add(new Point(SParts[0].X, SParts[0].Y - 1));
                break;
            default:
                break;
        }
    }
    public void GameOver()
    {
        IsAlive = false;
        Console.WriteLine("GameOver");
        Console.WriteLine("Нажмите на Q, чтобы выйти в главное меню");
    }
    public void SelfCollideCheck()
    {
        Point forepart = SParts[0];
        List<Point> temp = new List<Point>(SParts);
        temp.RemoveAt(0);
        if (temp.Contains(forepart))
        {
            GameOver();
        }
    }
    #endregion
}
