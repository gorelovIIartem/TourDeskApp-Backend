using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;

namespace WebApi.Configuration
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            if(exception is ArgumentException)
            {
                code = HttpStatusCode.BadRequest;
                Log.Error(exception, $"Exception was in{context.Request.Path}");
            }
            else if(exception is AuthenticationException)
            {
                code = HttpStatusCode.Unauthorized;
                Log.Error(exception,$"Exception was in {context.Request.Path}");
            }
            else if(exception is NotImplementedException)
            {
                code = HttpStatusCode.NotImplemented;
                Log.Error(exception, $"Exception was in{context.Request.Path}");
            }
            var result = JsonConvert.SerializeObject("Internal server error");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            Log.Error(exception, $"Exception was in{context.Request.Path}");
            return context.Response.WriteAsync(result);
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
    }
}
