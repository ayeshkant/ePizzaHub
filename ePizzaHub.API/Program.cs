
using ePizzaHub.API.HealthCheck;
using ePizzaHub.API.Middlewares;
using ePizzaHub.Application;
using ePizzaHub.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
namespace ePizzaHub.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpClient<ExternalAPIHealthCheck>();
            builder.Services.AddHealthChecks().AddCheck<ExternalAPIHealthCheck>("Regress API Health Status");
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi()
                .RegisterJwt(builder.Configuration);
            builder.Services.AddMemoryCache();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(ApplicationAssemblyMarker).Assembly);
            builder.Services.AddApplication();

            builder.Services.AddAutoMapper(typeof(InfrastructureAssemblyMarker).Assembly);
            builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(x => new
                        {
                            name = x.Key,
                            status = x.Value.Status.ToString(),
                            description = x.Value.Description,
                        }),
                        timeTaken = report.TotalDuration
                    });

                    await context.Response.WriteAsync(result);
                }

            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<CommonResponseMiddleware>();


            app.MapControllers();

            app.Run();
        }
    }
}
