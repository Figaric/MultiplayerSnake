using Microsoft.AspNetCore.SignalR.Client;
using MultiplayerSnake.Shared;

var connection = new HubConnectionBuilder()
    .WithUrl($"http://localhost:5000/hubs/gamehub", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI5NTkxNjA1YjA1ODU0MTY4ODQ4ZjAxYjRjZDYwOTFjMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkaW1hIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIxIiwiZXhwIjoxNjUzMzM5MTQ3LCJpc3MiOiJUZXN0SXNzIiwiYXVkIjoiVGVzdEF1ZCJ9.MoUW6p9RMYxUAOfjUw7GC9OFsb5hWxn_vItASHNZBPU");
    })
    .Build();

await connection.StartAsync();

connection.On<IList<Room>>(HubMethods.RoomsReceived, rooms =>
{
    rooms.ToList().ForEach(r => Console.WriteLine(r.Id));
});

connection.On<string>(HubMethods.RoomCreated, roomId =>
{
    Console.WriteLine("RoomId: " + roomId);
});

await connection.InvokeAsync(HubMethods.CreateRoom);

await connection.InvokeAsync(HubMethods.GetRooms, 0);

Console.ReadKey();