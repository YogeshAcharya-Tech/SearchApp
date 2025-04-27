using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SearchApp.Core.Interface;
using SearchApp.Infrastructure.Data;
using SearchApp.Infrastructure.Repositories;

namespace SearchApp.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection Services)
        {
            Services.AddDbContext<AppDbContext>((provider, options) => 
            {
                //options.UseSqlServer(provider.GetRequiredService<IOptionsSnapshot<ConnectionStringOptions>>().Value.DefaultConnection);
                options.UseSqlServer("Server=.;Database=EmployeeDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");
            });

            Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            Services.AddScoped<IAuthRepository, AuthRepository>();
            return Services;
        }
    }
}
