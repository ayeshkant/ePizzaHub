using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using ePizzaHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // Add AutoMapper profiles from this assembly
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddDbContext<ePizzaHubDBContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            // Register other application services here
            // services.AddTransient<IItemService, ItemService>();
            return services;
        }
    }
}
