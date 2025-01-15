using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventsApi.Middleware
{
    public class NotFoundMiddleware
    {
        public async Task HandleAsync(HttpContext context, Exception exception)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Recurso no encontrado.",
                Detailed = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
