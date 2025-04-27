using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchApp.Core.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreDI(this IServiceCollection Services)
        {
            return Services;
        }
    }
}
