using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiplayerSnake.Server.Dtos;
using MultiplayerSnake.Server.MediatR.Account;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server
{
    [ApiController]
    [Route("/account")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            var response = await _mediator.Send(registerDto);

            return Created(HttpContext.Request.Path, response);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var response = await _mediator.Send(loginDto);

            return Ok(response);
        }
    }
}
