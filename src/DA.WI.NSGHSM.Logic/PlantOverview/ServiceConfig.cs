using Microsoft.Extensions.DependencyInjection;

namespace DA.WI.NSGHSM.Logic.PlantOverview
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddPlantOverviewLogic(this IServiceCollection services)
        {
            services.AddLogic<RoughingMillDataLogic>();
            services.AddLogic<FinishingMillDataLogic>();
            services.AddLogic<CoolingDowncoilersDataLogic>();

            return services;
        }
    }
}