using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PersistenseOracle
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceOracle(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IOracleDbContext, OracleDbContext>(opt => opt.UseOracle(configuration.GetConnectionString("ODB"), b => b.UseOracleSQLCompatibility("11")), ServiceLifetime.Scoped);
            return services;
        }
    }
}
