using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Repositories.Extension
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            // veritabanı işlemleri
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionStrings = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

                options.UseSqlServer(connectionStrings!.SqlServer, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName);
                });

            });
            return services;

        }
    }
}
