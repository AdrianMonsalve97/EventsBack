using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventsApi.Middleware
{
    public class BadRequestMiddleware
    {
        public async Task HandleAsync(HttpContext context, Exception exception)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Solicitud inválida.",
                Detailed = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
