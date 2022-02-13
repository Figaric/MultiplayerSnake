using System;

namespace MultiplayerSnake.Shared;

public struct ColorManager
{
    private ushort Index;
    private ConsoleColor color;

    public ConsoleColor GetColor()
    {
        return color;
    }

    public void SetColor(ConsoleColor value)
    {
        color = value;
    }

    private readonly ushort[] Colors;
    public ColorManager() { Index = 0; Colors = new ushort[] { 12, 6, 14, 10, 11, 9, 13 }; color = ConsoleColor.White; }
    public ConsoleColor ReturnColor()
    {
        if (GetColor() == ConsoleColor.Black)
        {
            if (Index == 6)
            {
                Index = 0;
            }
            Index++;
            return (ConsoleColor)Colors[Index];
        }
        else
        {
            return GetColor();
        }
    }
}
