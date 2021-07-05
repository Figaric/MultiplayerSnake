using MediatR;

namespace MultiplayerSnake.Server
{
    public class UserLoginDto : IRequest<ResponseBase>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
