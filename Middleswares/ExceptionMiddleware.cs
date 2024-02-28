using System.Net;
using System.Text.Json;
using Talabat.APis.Errors;

namespace Talabat.APis.Middleswares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate Next, ILogger<ExceptionMiddleware> logger , IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }
        // must be any middleware have the fun +> InvokeAsync(HttpContect Context)+> context the detai of my reusa and re
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                // header of the object Response {Statuscode and Contenttype}
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                // body 
                var response = _env.IsDevelopment() ?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
                     new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message);// in case in prosuction
                                                                                                   // in production we log the eror in Database
                var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json=JsonSerializer.Serialize(response,option);
                await context.Response.WriteAsync(json);// writerAsyn to wirth the prop of classreo in body of reson and take json


            }
        }
    }
}
