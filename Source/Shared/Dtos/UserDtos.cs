namespace MultiplayerSnake.Shared;

public class UserRegisterDto
{
    public string UserName { get; set; }

    public string Password { get; set; }
}

public class UserLoginDto
{
    public string UserName { get; set; }

    public string Password { get; set; }
}

public class UserForgotPasswordDto
{
    public string UserName { get; set; }

    public string Email { get; set; }
}