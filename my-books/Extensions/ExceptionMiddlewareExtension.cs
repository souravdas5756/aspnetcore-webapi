using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace my_books.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var errorResponse = new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error.",
                            Detailed = contextFeature.Error.Message
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                    }
                });
            });
        }
    }
}
