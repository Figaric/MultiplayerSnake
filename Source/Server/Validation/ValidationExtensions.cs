using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server.Validation
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
