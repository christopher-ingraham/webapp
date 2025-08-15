using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Logic.AuxValue
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddAuxValueLogic(this IServiceCollection services)
        {
            services.AddLogic<AuxValueLogic>();

            return services;
        }
    }
}
