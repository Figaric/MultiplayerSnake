using System;

namespace MultiplayerSnake.Shared;

public struct ColorManager
{
    private ushort Index;
    public ConsoleColor Color { get; set; }
    private readonly ushort[] Colors;
    public ColorManager() { Index = 0; Colors = new ushort[] { 12, 6, 14, 10, 11, 9, 13 }; Color = ConsoleColor.White; }
    public ConsoleColor ReturnColor()
    {
        if (Color == ConsoleColor.Black)
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
            return Color;
        }
    }
}
