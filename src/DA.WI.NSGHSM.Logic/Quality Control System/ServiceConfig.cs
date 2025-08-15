using DA.WI.NSGHSM.Logic.QualityControlSystem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddQcsLogic(this IServiceCollection services)
        {
            services.AddLogic<RepHmPieceStatLogic>();
            services.AddLogic<RepHmPieceLogic>();
            services.AddLogic<TdbProcessCodeLogic>();
            services.AddLogic<TdbMaterialGradeLogic>();
            services.AddLogic<TdbGradeGroupLogic>();
            services.AddLogic<TdbAlloySpecLogic>();
            services.AddLogic<TdbAlloyLogic>();
            services.AddLogic<RepHmPieceTrendLogic>();
            services.AddLogic<RepHmTrendsViewLogic>();
            services.AddLogic<RepHmMapsViewLogic>();

            return services;
        }
    }
}