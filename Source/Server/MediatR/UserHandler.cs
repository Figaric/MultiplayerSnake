using Isopoh.Cryptography.Argon2;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server
{
    public static class UserCQRS
    {
        public record UserRequest(string username, string password) : IRequest<UserResponse>;

        public class UserHandler : IRequestHandler<UserRequest, UserResponse>
        {
            private readonly ApplicationDbContext _context;

            public UserHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<UserResponse> Handle(UserRequest request, CancellationToken cancellationToken)
            {
                if(string.IsNullOrWhiteSpace(request.username))
                {
                    return new UserResponse("Username is required.");
                }

                if (string.IsNullOrWhiteSpace(request.password))
                {
                    return new UserResponse("Password is required.");
                }

                string hashedPassword = Argon2.Hash(request.password);

                var user = new User
                {
                    Username = request.username,
                    Password = hashedPassword,
                };

                await _context.AddAsync(user);

                try
                {

                } 
                catch
                {

                }

                return new UserResponse("");
            }
        }

        public record UserResponse(string error);
    }
}
