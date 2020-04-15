using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using FindbookApi.AppExceptions;

namespace FindbookApi.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            if (ex is RequestArgumentException)
            {
                context.Response.StatusCode = (ex as RequestArgumentException).Code;
                return context.Response.WriteAsync($"{{\"error\":\"{ex.Message}\"}}");
            }
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Console.WriteLine("Internal Server Error");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
            Console.WriteLine(ex.StackTrace);
            return context.Response.WriteAsync($"{{\"error\":\"internal server error\"}}");
        }
    }

    public static class Extensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionMiddleware>();
    }
}