using Microsoft.AspNetCore.SignalR.Client;
using MultiplayerSnake.Shared;

var connection = new HubConnectionBuilder()
    .WithUrl($"http://localhost:5000/hubs/gamehub", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI5NTkxNjA1YjA1ODU0MTY4ODQ4ZjAxYjRjZDYwOTFjMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkaW1hIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIxIiwiZXhwIjoxNjUzMzM5MTQ3LCJpc3MiOiJUZXN0SXNzIiwiYXVkIjoiVGVzdEF1ZCJ9.MoUW6p9RMYxUAOfjUw7GC9OFsb5hWxn_vItASHNZBPU");
    })
    .Build();

connection.On<string>(HubMethods.CreateRoom, roomId => Console.WriteLine("RoomId: " + roomId));

connection.On<IList<Player>>(HubMethods.JoinRoom, players => 
{
    players.ToList()
        .ForEach(p => Console.WriteLine(p.Nickname));
});

connection.On<IList<Player>>(HubMethods.LeaveRoom, players => 
{
    players.ToList()
        .ForEach(p => Console.WriteLine(p.Nickname));
});

await connection.StartAsync();



Console.ReadKey();