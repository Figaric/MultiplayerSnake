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

        public async void Register(string username, string password)
        {
            var response = await _mediator.Send(new UserCQRS.UserRequest(username, password));

            await Clients.Client(Context.ConnectionId).SendAsync("register-response", response.error);
        }
    }
}
