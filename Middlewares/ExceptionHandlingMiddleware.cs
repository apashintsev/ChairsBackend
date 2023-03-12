using ChairsBackend.Models;
using Newtonsoft.Json;
using System.Net;

namespace ChairsBackend.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string exMsg, HttpStatusCode httpStatusCode, string message)
        {
            _logger.LogError(exMsg);

            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            BaseResponse<object> resp = new(null);
            resp.AddError(message);

            var json = JsonConvert.SerializeObject(resp);
            await response.WriteAsync(json);
        }
    }
}