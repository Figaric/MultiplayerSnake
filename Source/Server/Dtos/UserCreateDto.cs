using MediatR;

namespace MultiplayerSnake.Server
{
    public record UserCreateDto(string Username, string Password) : IRequest<CQRSResponse>;
}
