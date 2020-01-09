using Facemash.API.Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facemash.API
{
    public static class IOC
    {
        public static IServiceCollection ConfigureIOC(this IServiceCollection services)
            => services
                .AddTransient<ICatManagement, CatManagement>();
    }
}

