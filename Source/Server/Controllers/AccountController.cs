using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ApiRoutes.Register)]
        public async Task<IActionResult> RegisterAsync(UserRegisterDto registerDto)
        {
            ResponseBase response = await _mediator.Send(registerDto);

            return Created(HttpContext.Request.Path, response);
        }

        [HttpPost(ApiRoutes.Login)]
        public async Task<IActionResult> LoginAsync(UserLoginDto loginDto)
        {
            ResponseBase response = await _mediator.Send(loginDto);

            return Ok(response);
        }
    }
}
