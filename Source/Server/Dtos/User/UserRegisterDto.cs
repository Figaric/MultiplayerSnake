using MediatR;

namespace MultiplayerSnake.Server
{
    public class UserRegisterDto : IRequest<ResponseBase>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
