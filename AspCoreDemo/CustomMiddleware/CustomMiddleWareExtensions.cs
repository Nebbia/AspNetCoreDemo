using Microsoft.AspNetCore.Builder;

namespace AspCoreDemo.CustomMiddleware
{
    public static class CustomMiddleWareExtensions
    {
        public static IApplicationBuilder UseCustomMyCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }
}