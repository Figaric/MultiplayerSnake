using Microsoft.EntityFrameworkCore;

namespace MultiplayerSnake.Server;

[Index(nameof(UserName), IsUnique = true)]
public class User
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }
}