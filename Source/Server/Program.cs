using MultiplayerSnake.Server;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MultiplayerSnake.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(cfg => cfg.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>(ServiceLifetime.Transient);
builder.Services.AddTransient<IValidationManager, ValidationManager>();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"])
                )
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure middlewares

app.UseExceptionHandler(a =>
{
    a.Run(async context =>
    {
        await context.Response.WriteAsJsonAsync(new { Error = "djwidjwid" });
    });
});

app.UseConditionalMiddleware<ValidationMiddleware>(ApiEndpoints.AccountRoute);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<GameHub>("/hubs/gamehub");

app.Run();