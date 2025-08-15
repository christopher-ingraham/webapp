using Microsoft.Extensions.DependencyInjection;
using DA.WI.NSGHSM.Repo._Core;
using DA.WI.NSGHSM.IRepo.PlantOverview;

namespace DA.WI.NSGHSM.Repo.PlantOverview
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddPlantOverviewServices(this IServiceCollection services)
        {
            services.AddRepo<IRoughingMillDataRepo<ReportDataSource>, RoughingMillDataRepo<ReportDataSource>>();
            services.AddRepo<IFinishingMillDataRepo<ReportDataSource>, FinishingMillDataRepo<ReportDataSource>>();
            services.AddRepo<ICoolingDowncoilersDataRepo<ReportDataSource>, CoolingDowncoilersDataRepo<ReportDataSource>>();

            return services;
        }

    }
}