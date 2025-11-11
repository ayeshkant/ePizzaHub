
using ePizzaHub.Application;
using ePizzaHub.Infrastructure;
namespace ePizzaHub.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddAutoMapper(typeof(ApplicationAssemblyMarker).Assembly);
            builder.Services.AddApplication();

            builder.Services.AddAutoMapper(typeof(InfrastructureAssemblyMarker).Assembly);
            builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
