using MultiplayerSnake.Server;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MultiplayerSnake.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Host.ConfigureLogging(logging =>
{
    logging.AddConfiguration(builder.Configuration);
    logging.AddConsole();

    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information);
    logging.AddFilter("Microsoft.EntityFrameworkCore.Update", LogLevel.None);
    logging.AddFilter("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogLevel.None);
});

builder.Services.AddSignalR();
builder.Services.AddControllers(options => options.Filters.Add<ResponseMappingFilter>());
builder.Services.AddDbContext<ApplicationDbContext>(cfg =>
    {
        cfg.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
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
        app.Logger.LogWarning("HElLOOOOOOOO");
        if (context.Request.Path.StartsWithSegments('/' + ApiEndpoints.AccountRoute + '/' + ApiEndpoints.RegisterRoute))
        {
            var dto = (UserRegisterDto)context.Items["RegisterDto"];

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if ((await dbContext.Users.FirstOrDefaultAsync(u => dto.UserName == u.UserName)) != null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        errors = new List<FieldError>
                        {
                            new FieldError
                            {
                                Field = nameof(dto.UserName),
                                Message = "This UserName is already taken."
                            }
                        }
                    });
                }
            }
        }
    });
});

app.UseConditionalMiddleware<ValidationMiddleware>(ApiEndpoints.AccountRoute);
app.UseMiddleware<LoggingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHub<GameHub>("/hubs/gamehub");

app.Run();