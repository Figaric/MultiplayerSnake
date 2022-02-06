using System;

namespace MultiplayerSnake.Client;

struct RainbowColor
{
    private ushort Index;
    public RainbowColor() { Index = 1; }
    public ConsoleColor ReturnColor()
    {
        if (Index == 16)
        {
            Index = 1;
        }
        Index++;
        return (ConsoleColor)Index - 1;
    }
}
