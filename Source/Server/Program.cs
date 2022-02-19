using MultiplayerSnake.Server;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

// Register controllers with needed filters
builder.Services.AddControllersWithFilters();

// Register validation stuff
builder.Services.AddValidation();

// Register database context
builder.Services.AddDatabase();

#endregion

var app = builder.Build();

#region Configure middlewares

app.UseAuthorization();
app.MapControllers();

#endregion

app.Run();