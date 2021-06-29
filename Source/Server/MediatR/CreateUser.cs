using Isopoh.Cryptography.Argon2;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server
{
    public static class UserCQRS
    {
        public class UserHandler : IRequestHandler<UserCreateDto, CQRSResponse>
        {
            private readonly UserManager _manager;

            public UserHandler(UserManager manager)
            {
                _manager = manager;
            }

            public async Task<CQRSResponse> Handle(UserCreateDto request, CancellationToken cancellationToken)
            {
                var user = new User
                {
                    Username = request.Username
                };

                _manager.AddUser(user);

                return CQRSResponse.Succeed();
            }
        }
    }
}
