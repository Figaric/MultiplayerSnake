using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace MultiplayerSnake.Server
{
    public static class ValidationExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<Startup>()
                .AddClasses(classes => classes.AssignableTo<IValidator>())
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
        }
    }
}
