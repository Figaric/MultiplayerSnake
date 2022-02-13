namespace MultiplayerSnake.Shared;

class SnakeSharedProperties
{
    #region Properties
    private List<Point> SParts { get; set; }
    private List<Point> FoodCoords { get; set; }
    public double Speed { get; set; }
    private int Score { get; set; }
    public bool IsAlive { get; set; }
    public ConsoleColor Color { get; set; }
    private ColorManager Rainbow;
    #endregion

}
