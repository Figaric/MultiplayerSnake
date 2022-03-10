using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MultiplayerSnake.Server;

public static class ServiceExtensions
{
    public static void AddCustomLogging(this WebApplicationBuilder builder)
    {
        // builder.Host.ConfigureLogging(logging =>
        // {
        //    logging.AddConfiguration(builder.Configuration);
        //    logging.AddConsole();

        //    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
        //    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information);
        //    logging.AddFilter("Microsoft.EntityFrameworkCore.Update", LogLevel.None);
        //    logging.AddFilter("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogLevel.None);
        // });
    }

    public static void AddControllersWithFilters(this IServiceCollection services)
    {
        services.AddControllers(options => options.Filters.Add<ResponseMappingFilter>());
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>(ServiceLifetime.Transient);
        services.AddTransient<IValidationManager, ValidationManager>();
    }

    public static void AddDatabase(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)
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
    }
}
