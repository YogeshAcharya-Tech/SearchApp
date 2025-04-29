using SearchApp.Application.DependencyInjection;
using SearchApp.Infrastructure.DependencyInjection;

namespace SearchApp.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection Services)
        {
            Services.AddSingleton<ILogger, Logger>();
            Services.AddApplicationDI().AddInfrastructureDI();
            return Services;
        }
    }
}
