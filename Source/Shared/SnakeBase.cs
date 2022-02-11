namespace MultiplayerSnake.Shared;

public class SnakeBase
{
    public IEnumerable<Point> SnakePos { get; set; }

    public ConsoleColor SnakeColor { get; set; }
}
