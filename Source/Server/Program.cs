using MultiplayerSnake.Server;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

builder.Services.AddSignalR();

// Map jwt settings from appsettings.json to the JwtSettings object
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

// Register controllers with needed filters
builder.Services.AddControllersWithFilters();

// Register validation stuff
builder.Services.AddValidation();

// Register database context
builder.Services.AddDatabase();

// Register jwt authenticators
builder.Services.AddJwtAuthentication();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

#endregion

var app = builder.Build();

#region Configure middlewares

app.UseAuthorization();
app.MapControllers();
app.MapHub<GameHub>("/hubs/gamehub");

#endregion

app.Run();