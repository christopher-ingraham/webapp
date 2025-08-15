using DA.WI.NSGHSM.Logic.Production;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Logic.Production
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddProductionLogic(this IServiceCollection services)
        {
            services.AddLogic<HrmAnalysisDataLogic>();
            services.AddLogic<HrmHeatLogic>();
            services.AddLogic<HrmInputPieceLogic>();
            services.AddLogic<HrmJobLogic>();
            services.AddLogic<TdbMaterialSpecLogic>();
            services.AddLogic<ProducedCoilsLogic>();
            services.AddLogic<UsedSetupLogic>();
            services.AddLogic<ExitSaddlesLogic>();
            services.AddLogic<RmlCrewLogic>();
            services.AddLogic<RepHmSetupLogic>();

            return services;
        }
    }
}