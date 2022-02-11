using System;

namespace MultiplayerSnake.Shared;

public struct RainbowColor
{
    private ushort Index;
    private readonly ushort[] Colors;
    public RainbowColor() { Index = 0; Colors = new ushort[] { 12, 6, 14, 10, 11, 9, 13 }; }
    public ConsoleColor ReturnColor()
    {
        if (Index == 6)
        {
            Index = 0;
        }
        Index++;
        return (ConsoleColor)Colors[Index];
    }
}
