using Microsoft.Extensions.DependencyInjection;

namespace SearchApp.Core.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreDI(this IServiceCollection Services)
        {
            Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
            return Services;
        }
    }
}
