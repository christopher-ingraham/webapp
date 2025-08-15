using Microsoft.Extensions.DependencyInjection;
using DA.WI.NSGHSM.Repo._Core;
using DA.WI.NSGHSM.IRepo.Report;

namespace DA.WI.NSGHSM.Repo.Report
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddReportServices(this IServiceCollection services)
        {
            services.AddRepo<ICoilGeneralReportRepo<ReportDataSource>, CoilGeneralReportRepo<ReportDataSource>>();
            services.AddRepo<IShiftReportRepo<ReportDataSource>, ShiftReportRepo<ReportDataSource>>();
            services.AddRepo<IStoppageReportRepo<ReportDataSource>, StoppageReportRepo<ReportDataSource>>();
            services.AddRepo<IPracticeReportRepo<ReportDataSource>, PracticeReportRepo<ReportDataSource>>();
 
            return services;
        }

    }
}