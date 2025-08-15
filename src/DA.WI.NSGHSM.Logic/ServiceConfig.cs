using DA.WI.NSGHSM.Core.App;
using DA.WI.NSGHSM.Logic._Core.Configuration;
using DA.WI.NSGHSM.Logic.AuxValue;
using DA.WI.NSGHSM.Logic.Report;
using DA.WI.NSGHSM.Logic.QualityControlSystem;
using DA.WI.NSGHSM.Logic.Production;
using DA.WI.NSGHSM.Logic.Messaging;
using DA.WI.NSGHSM.Repo;
using Microsoft.Extensions.DependencyInjection;
using DA.WI.NSGHSM.Logic.PlantOverview;

namespace DA.WI.NSGHSM.Logic
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddLogicServices(this IServiceCollection services, Configuration config)
        {
            services.AddLogic<UserLogic>();
            services.AddReportLogic();
            services.AddQcsLogic();
            services.AddAuxValueLogic();
            services.AddProductionLogic();
            services.AddPlantOverviewLogic();
            services.AddMessagingLogic();
            services.AddRepoServices(config);

            return services;
        }

        public static IServiceCollection AddLogic<TLogic>(this IServiceCollection services)
            where TLogic : class
        {
            return services.AddTransient<TLogic>();
        }
    }
}
