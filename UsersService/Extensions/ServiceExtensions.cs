using Microsoft.EntityFrameworkCore;
using UsersService.Repositories;
using UsersService.Services;

namespace UsersService.Extensions
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
              builder.AllowAnyOrigin()
                     .AllowAnyMethod() //.WithMethods("GET", "POST")
                     .AllowAnyHeader());
          });

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
                    services.AddDbContext<UsersServiceDbContext>(opts =>
                            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        /// <summary> Регистрация менеджера репозитория </summary>
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
                                      services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
                                      services.AddScoped<IServiceManager, ServiceManager>();
    }
}
