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
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IUserTokenService, UserTokenService>();
            services.AddScoped<IPaymentService, PaymentService>();
            // Register other application services here
            // services.AddTransient<IItemService, ItemService>();
            return services;
        }
    }
}
