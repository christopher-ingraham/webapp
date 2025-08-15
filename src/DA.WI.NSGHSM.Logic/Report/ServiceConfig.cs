using DA.WI.NSGHSM.Logic.QualityControlSystem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Logic.Report
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddReportLogic(this IServiceCollection services)
        {
            services.AddLogic<CoilGeneralReportLogic>();
            services.AddLogic<ShiftReportLogic>();
            services.AddLogic<StoppageReportLogic>();
            services.AddLogic<PracticeReportLogic>();

            return services;
        }
    }
}