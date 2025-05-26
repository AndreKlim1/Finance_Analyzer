

namespace NotificationsService.Extensions
{
    /// <summary> Класс расширения для регистрации различных служб </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Настроиваю CORS в приложении. CORS(Cross-Origin Resource Sharing) — это механизм предоставления или ограничения
        /// права доступа к приложениям из разных доменов
        /// </summary>
        public static void ConfigureCors(this IServiceCollection services) =>
          services.AddCors(options =>
          {
              options.AddPolicy("CorsPolicy", builder =>
              builder.WithOrigins("http://localhost:3000")
                     .AllowAnyMethod() //.WithMethods("GET", "POST")
                     .AllowAnyHeader()
                     .AllowCredentials());
          });


    }
}
