using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace EventsApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Delegar a middlewares específicos según el error
                var statusCode = GetStatusCode(ex);
                switch (statusCode)
                {
                    case HttpStatusCode.BadRequest:
                        await new BadRequestMiddleware().HandleAsync(context, ex);
                        break;
                    case HttpStatusCode.Unauthorized:
                        await new UnauthorizedMiddleware().HandleAsync(context, ex);
                        break;
                    case HttpStatusCode.NotFound:
                        await new NotFoundMiddleware().HandleAsync(context, ex);
                        break;
                    default:
                        await new InternalServerErrorMiddleware().HandleAsync(context, ex);
                        break;
                }
            }
        }

        private HttpStatusCode GetStatusCode(Exception ex)
        {
            return ex switch
            {
                ArgumentNullException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}
