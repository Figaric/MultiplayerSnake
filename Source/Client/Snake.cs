﻿using System;
using System.Collections.Generic;

namespace snake
{
    struct Point
    {

        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return $"{x} : {y}";
        }
    }

    class Snake
    {
        #region Properties
        public int Direction { get; set; }
        public List<Point> sParts { get; set; }
        public char[,] Map { get; set; }
        public List<Point> foodCoords { get; set; }
        public double Speed { get; set; }
        public int mapSize { get; set; }
        public int Score { get; set; }
        public int iterCount { get; set; }
        public Random rnd { get; set; }
        #endregion

        #region Constructor
        public Snake(int x)
        {
            Direction = 0;
            Map = new char[x, x];
            mapSize = (int)Math.Sqrt(Map.Length) - 1;
            for (int i = 0; i < mapSize; i++) // Generating map bounds
            {
                Map[0, i] = '#';
                Map[i, 0] = '#';
                Map[mapSize, i] = '#';
                Map[i, mapSize] = '#';
            }
            Map[mapSize, mapSize] = '#';
            sParts = new List<Point>() { new Point((mapSize + 1) / 2, (mapSize + 1) / 2) };
            foodCoords = new List<Point>() { };
            Score = 0;
            Speed = 1;
            rnd = new Random();
        }
        #endregion

        #region Funcs
        public void Draw()
        {
            Console.Clear();
            for (int i = 0; i < mapSize + 1; i++) // Simple 2D array display
            {
                for (int j = 0; j < mapSize + 1; j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.Write("\n");
            }
            Console.WriteLine($"Score: {Score}");
            Console.WriteLine($"Speed: {Speed}");
        }

        public void Update()
        {
            Point lCoords = sParts[0];
            switch (Direction)
            {
                case 0:
                    sParts.Remove(sParts[sParts.Count - 1]);
                    sParts.Reverse();
                    sParts.Add(new Point(lCoords.x + 1, lCoords.y));
                    sParts.Reverse();
                    break;
                case 1:
                    sParts.Remove(sParts[sParts.Count - 1]);
                    sParts.Reverse();
                    sParts.Add(new Point(lCoords.x, lCoords.y + 1));
                    sParts.Reverse();
                    break;
                case 2:
                    sParts.Remove(sParts[sParts.Count - 1]);
                    sParts.Reverse();
                    sParts.Add(new Point(lCoords.x - 1, lCoords.y));
                    sParts.Reverse();
                    break;
                case 3:
                    sParts.Remove(sParts[sParts.Count - 1]);
                    sParts.Reverse();
                    sParts.Add(new Point(lCoords.x, lCoords.y - 1));
                    sParts.Reverse();
                    break;
                default:
                    break;
            }
            mapConstruct();
            if (sParts[0].x == mapSize || sParts[0].y == mapSize || sParts[0].x == 0 || sParts[0].y == 0) // Checking if the player is out of bounds
            {
                Console.WriteLine("GameOver");
                Console.Read();
            }
            iterCount++;
            if (iterCount % mapSize*Speed == 0 && Speed < 50)
            {
                Speed=Math.Round(Speed*1.1, 2);
            }
            generateFood();
            Draw();
        }

        private void generateFood()
        {
            if (iterCount % mapSize == 0 || foodCoords.Count == 0)
            {
                foodCoords.Add(new Point(rnd.Next(1, mapSize), rnd.Next(1, mapSize)));
            }
        }
        private void mapConstruct()
        {
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    Map[i, j] = ' ';
                    foreach (var part in sParts) // Map construction based on the position of the snake parts
                    {
                        foreach (var food in foodCoords)
                        {
                            if (i == food.x && j == food.y)
                            {
                                Map[i, j] = '+';
                            }
                            if (food.x == part.x && food.y == part.y)
                            {
                                foodCoords.Remove(food);
                                Map[food.x, food.y] = '*';
                                snakeGrowth();
                                Score++;
                                return;
                            }
                        }
                        if (i == part.x && j == part.y)
                        {
                            Map[i, j] = '*';
                        }
                    }
                }
            }
        }

        private void snakeGrowth()
        {
            switch (Direction)
            {
                case 0:
                    sParts.Add(new Point(sParts[0].x + 1, sParts[0].y));
                    break;
                case 1:
                    sParts.Add(new Point(sParts[0].x, sParts[0].y + 1));
                    break;
                case 2:
                    sParts.Add(new Point(sParts[0].x - 1, sParts[0].y));
                    break;
                case 3:
                    sParts.Add(new Point(sParts[0].x, sParts[0].y - 1));
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}