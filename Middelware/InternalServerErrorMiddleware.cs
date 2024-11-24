using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventsApi.Middleware
{
    public class InternalServerErrorMiddleware
    {
        public async Task HandleAsync(HttpContext context, Exception exception)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Error interno del servidor.",
                Detailed = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
