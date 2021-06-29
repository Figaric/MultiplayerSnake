using Isopoh.Cryptography.Argon2;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiplayerSnake.Server.Dtos;
using MultiplayerSnake.Server.Entities;
using MultiplayerSnake.Server.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server.MediatR.Account
{
    public static class Register
    {
        public class Handler : IRequestHandler<UserRegisterDto, ResponseBase>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ResponseBase> Handle(UserRegisterDto request, CancellationToken cancellationToken)
            {
                string hashedPassword = Argon2.Hash(request.Password);

                var user = new User
                {
                    Username = request.Username,
                    Password = hashedPassword
                };

                await _context.Users.AddAsync(user);

                try
                {
                    await _context.SaveChangesAsync();
                } catch
                {
                    if(await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username) != null)
                    {
                        return ResponseBase.Failed(new FieldError
                        {
                            Field = nameof(user.Username),
                            Message = "This username is already taken."
                        });
                    }
                }

                return ResponseBase.Succeed;
            }
        }
    }
}
