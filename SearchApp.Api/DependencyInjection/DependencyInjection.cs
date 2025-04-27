using SearchApp.Application.DependencyInjection;
using SearchApp.Infrastructure.DependencyInjection;

namespace SearchApp.Api.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection Services)
        {
            Services.AddApplicationDI().AddInfrastructureDI();
            return Services;
        }
    }
}
