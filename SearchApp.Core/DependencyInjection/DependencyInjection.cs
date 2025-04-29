using Microsoft.Extensions.DependencyInjection;

namespace SearchApp.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainDI(this IServiceCollection Services)
        {
            Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
            return Services;
        }
    }
}
