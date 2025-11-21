using ePizzaHub.Application.DTOs.Response;
using System.Text.Json;

namespace ePizzaHub.API.Middlewares
{
    public class CommonResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CommonResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var originalBody = httpContext.Response.Body;
            using var memoryStream = new MemoryStream();
            httpContext.Response.Body = memoryStream;

            await _next(httpContext);
            if (httpContext.Response.ContentType!=null &&
                httpContext.Response.ContentType.Contains("application/json"))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
                var responseObject = new ApiResponseModelDto<object>(
                    issuccess: httpContext.Response.StatusCode is >= 200 and < 300,
                    data: JsonSerializer.Deserialize<object>(responseBody)!,
                    message: "Request completed"
                );
                var jsonResponse = JsonSerializer.Serialize(responseObject);
                httpContext.Response.Body = originalBody;
                await httpContext.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
