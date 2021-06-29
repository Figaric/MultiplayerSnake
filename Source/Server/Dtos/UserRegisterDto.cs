using MediatR;
using MultiplayerSnake.Server.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server.Dtos
{
    public class UserRegisterDto : IRequest<ResponseBase>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
