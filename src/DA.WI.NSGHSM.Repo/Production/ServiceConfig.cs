using Microsoft.Extensions.DependencyInjection;
using DA.WI.NSGHSM.Repo._Core;
using DA.WI.NSGHSM.IRepo.Production;

namespace DA.WI.NSGHSM.Repo.Production
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddProductionServices(this IServiceCollection services)
        {
            services.AddRepo<IHrmJobRepo<ReportDataSource>, HrmJobRepo<ReportDataSource>>();
            services.AddRepo<IHrmInputPieceRepo<ReportDataSource>, HrmInputPieceRepo<ReportDataSource>>();
            services.AddRepo<IHrmAnalysisDataRepo<ReportDataSource>, HrmAnalysisDataRepo<ReportDataSource>>();   
            services.AddRepo<IHrmHeatRepo<ReportDataSource>, HrmHeatRepo<ReportDataSource>>();
            services.AddRepo<ITdbMaterialSpecRepo<ReportDataSource>, TdbMaterialSpecRepo<ReportDataSource>>();
            services.AddRepo<IProducedCoilsRepo<ReportDataSource>, ProducedCoilsRepo<ReportDataSource>>();
            services.AddRepo<IUsedSetupRepo<ReportDataSource>, UsedSetupRepo<ReportDataSource>>();
            services.AddRepo<IExitSaddlesRepo<ReportDataSource>, ExitSaddlesRepo<ReportDataSource>>();
            services.AddRepo<IRmlCrewRepo<ReportDataSource>, RmlCrewRepo<ReportDataSource>>();
            services.AddRepo<IRepHmSetupRepo<ReportDataSource>, RepHmSetupRepo<ReportDataSource>>();
            return services;
        }

    }
}