using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using UsersService.Models.Errors;

namespace CaregoryAccountService.Extensions
{
    /// <summary>
    /// Метод расширения для возврата кодов и сообщений ответов при запросах
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Метод расширения, в котором зарегистрировано промежуточное ПО UseExceptionHandler для 
        /// заполнения кода состояния и типа содержимого ответа, и возвращения ответ через error-модель ErrorDetails
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureExceptionHandler(this WebApplication app)
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
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });

        }
    }
}
