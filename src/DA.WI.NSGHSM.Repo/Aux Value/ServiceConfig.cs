using Microsoft.Extensions.DependencyInjection;
using DA.WI.NSGHSM.Repo._Core;
using DA.WI.NSGHSM.IRepo.AuxValue;

namespace DA.WI.NSGHSM.Repo.AuxValue
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddAuxValueServices(this IServiceCollection services)
        {
            services.AddRepo<IAuxValueRepo<ReportDataSource>, AuxValueRepo<ReportDataSource>>();

            return services;
        }

    }
}