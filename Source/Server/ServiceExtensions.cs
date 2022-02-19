using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace MultiplayerSnake.Server;

public static class ServiceExtensions
{
    public static void AddControllersWithFilters(this IServiceCollection services)
    {
        services.AddControllers(options => options.Filters.Add<ResponseMappingFilter>());
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();
        services.AddTransient<IValidationManager, ValidationManager>();
    }

    public static void AddDatabase(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}
