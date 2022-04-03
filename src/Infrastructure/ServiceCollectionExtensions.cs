using ApplicationCore.Common.Interfaces;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbC>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbC).Assembly.FullName)));

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddSingleton(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddSingleton(typeof(IRepository<>), typeof(EfRepository<>));

        return services;
    }
}
