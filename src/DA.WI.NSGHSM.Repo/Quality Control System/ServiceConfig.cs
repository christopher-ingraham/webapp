using Microsoft.Extensions.DependencyInjection;
using DA.WI.NSGHSM.Repo._Core;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;

namespace DA.WI.NSGHSM.Repo.QualityControlSystem
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddQcsServices(this IServiceCollection services)
        {
            services.AddRepo<IRepHmPieceStatRepo<ReportDataSource>, RepHmPieceStatRepo<ReportDataSource>>();
            services.AddRepo<IRepHmPieceRepo<ReportDataSource>, RepHmPieceRepo<ReportDataSource>>();
            services.AddRepo<ITdbProcessCodeRepo<ReportDataSource>, TdbProcessCodeRepo<ReportDataSource>>();
            services.AddRepo<ITdbMaterialGradeRepo<ReportDataSource>, TdbMaterialGradeRepo<ReportDataSource>>();
            services.AddRepo<ITdbGradeGroupRepo<ReportDataSource>, TdbGradeGroupRepo<ReportDataSource>>();
            services.AddRepo<ITdbAlloySpecRepo<ReportDataSource>, TdbAlloySpecRepo<ReportDataSource>>();
            services.AddRepo<ITdbAlloyRepo<ReportDataSource>, TdbAlloyRepo<ReportDataSource>>();
            services.AddRepo<IRepHmPieceTrendRepo<ReportDataSource>, RepHmPieceTrendRepo<ReportDataSource>>();
            services.AddRepo<IRepHmTrendsViewRepo<ReportDataSource>, RepHmTrendsViewRepo<ReportDataSource>>();
            services.AddRepo<IRepHmMapsViewRepo<ReportDataSource>, RepHmMapsViewRepo<ReportDataSource>>();

            return services;
        }

    }
}