using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server.Hubs
{
    public class AccountHub : Hub
    {
        private readonly IMediator _mediator;

        public AccountHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("New Connection");

            return base.OnConnectedAsync();
        }

        public async void RequestCreateUser(string username, string password)
        {
            var response = await _mediator.Send(new UserCreateDto(username, password));

            await Clients.Client(Context.ConnectionId).SendAsync("ResponseCreateUser", response);
        }
    }
}
