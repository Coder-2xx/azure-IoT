using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace azIoT.Core.Apps.WebApi.Middlewares
{

    public static class AzureADAuthMiddlewareExtension
    {
        public static IApplicationBuilder UseAzureADAuthMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AzureADAuthMiddleware>();
        }
    }

    public class AzureADAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AzureADAuthMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //await httpContext.Response.WriteAsync("Invalid token.");
            //await _next.Invoke(httpContext);
        }

    }
}
