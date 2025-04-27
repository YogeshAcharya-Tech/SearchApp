using Microsoft.Extensions.DependencyInjection;

namespace SearchApp.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection Services)
        {
            Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(typeof (DependencyInjection).Assembly));
            return Services;
        }
    }
}
