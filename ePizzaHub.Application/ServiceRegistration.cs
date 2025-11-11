using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add AutoMapper profiles from this assembly
            services.AddScoped<IItemService, ItemService>();
            // Register other application services here
            // services.AddTransient<IItemService, ItemService>();
            return services;
        }
    }
}
