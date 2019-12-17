using Microsoft.AspNetCore.Builder;

namespace WebApi.Configuration
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
