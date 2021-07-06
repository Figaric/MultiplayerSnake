using AutoMapper;
using Isopoh.Cryptography.Argon2;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server
{
    public static class Register
    {
        public class Handler : IRequestHandler<UserRegisterDto, ResponseBase>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ResponseBase> Handle(UserRegisterDto request, CancellationToken cancellationToken)
            {
                // Validation stuff is in the Validation/UserValidator.cs

                string hashedPassword = Argon2.Hash(request.Password);

                User user = _mapper.Map<User>(request);
                user.Password = hashedPassword;

                await _context.Users.AddAsync(user);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    if (await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username) != null)
                    {
                        return ResponseBase.Failed(new FieldError
                        {
                            Field = nameof(user.Username),
                            Message = "This username is already taken."
                        });
                    }
                }

                return ResponseBase.Succeed(System.Net.HttpStatusCode.Created);
            }
        }
    }
}
