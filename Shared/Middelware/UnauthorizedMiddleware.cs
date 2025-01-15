using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventsApi.Middleware
{
    public class UnauthorizedMiddleware
    {
        public async Task HandleAsync(HttpContext context, Exception exception)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "Acceso no autorizado.",
                Detailed = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
