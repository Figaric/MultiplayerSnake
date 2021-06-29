using Isopoh.Cryptography.Argon2;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiplayerSnake.Server.Responses;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System;
using MultiplayerSnake.Server.Dtos;
using System.Linq;

namespace MultiplayerSnake.Server
{
    public static class Login
    {
        public class Handler : IRequestHandler<UserLoginDto, ResponseBase>
        {
            private readonly ApplicationDbContext _context;
            private readonly JwtManager _jwt;

            public Handler(ApplicationDbContext context, JwtManager jwt)
            {
                _context = context;
                _jwt = jwt;
            }

            public async Task<ResponseBase> Handle(UserLoginDto request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                if (user == null)
                {
                    return ResponseBase.Failed(new FieldError
                    {
                        Field = nameof(request.Username),
                        Message = "User with this username doesn't exist."
                    });
                }

                if (!Argon2.Verify(user.Password, request.Password))
                {
                    return ResponseBase.Failed(new FieldError
                    {
                        Field = nameof(request.Password),
                        Message = "Incorrect password."
                    });
                }

                string accessToken = _jwt.CreateAccessToken(user.Id);

                return ResponseBase<string>.Succeed(accessToken);
            }
        }
    }
}
