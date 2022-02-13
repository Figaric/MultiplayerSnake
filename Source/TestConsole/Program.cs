using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:5001/hubs/gamehub", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJiMTM0NWNiZThmYzQ0ZjUwODA2ZjVjZDE0YWE5ZWU0MyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ0aWtob24iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJleHAiOjE2NTIzOTQyMjYsImlzcyI6IlRlc3RJc3MiLCJhdWQiOiJUZXN0QXVkIn0.DE-s56Wgf5MxRd_HPlVD3RkvemvR5zV-criTaIxIFI8");
    })
    .Build();

connection.On<string>("NewPlayer", name =>
{
    Console.WriteLine("New player in the room: " + name);
});

connection.On<string>("CreateRoomResponse", roomId =>
{
    Console.WriteLine("Room id: " + roomId);
});

await connection.StartAsync();

Console.WriteLine("Whether you wanna host or join room?");
int response = int.Parse(Console.ReadLine());

if(response == 0)
{
    await connection.InvokeAsync("CreateRoom");

    Console.ReadKey();
}
else if(response == 1)
{
    Console.Write("Enter room id: ");
    string roomId = Console.ReadLine();
    Console.Clear();

    await connection.InvokeAsync("JoinRoom", roomId);
}