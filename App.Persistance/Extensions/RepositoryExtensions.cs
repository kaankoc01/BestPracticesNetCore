using App.Application.Contracts.Persistence;
using App.Domain.Options;
using App.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Persistence.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            // veritabanı işlemleri /// veritabanı varsa scoped olur.
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionStrings = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

                options.UseSqlServer(connectionStrings!.SqlServer, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.MigrationsAssembly(typeof(PersistenceAssembly).Assembly.FullName);
                });

                options.AddInterceptors(new AuditDbContextIntercepter());

            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;

        }
    }
}
