using DA.WI.NSGHSM.Core.App;
using DA.WI.NSGHSM.Repo._Core;
using DA.WI.NSGHSM.Repo.Report;
using DA.WI.NSGHSM.Repo.AuxValue;
using DA.WI.NSGHSM.Repo.Production;
using DA.WI.NSGHSM.Repo.QualityControlSystem;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using DA.WI.NSGHSM.Repo.PlantOverview;

[assembly: InternalsVisibleTo("DA.WI.NSGHSM.Repo.Test")]
namespace DA.WI.NSGHSM.Repo
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddRepoServices(this IServiceCollection services, Configuration config)
        {

            services
                .AddDataSource<ConfigurationDataSource>(config)
                .AddDataSource<ReportDataSource>(config);

            services
                .AddConfigurationRepos<ConfigurationDataSource>()
                .AddAuxValueServices()
                .AddQcsServices()
                .AddPlantOverviewServices()
                .AddProductionServices()
                .AddReportServices();

            return services;
        }

        public static IServiceCollection AddRepoServicesForIdentityServer(this IServiceCollection services, Configuration config)
        {
            services
                .AddDataSource<ConfigurationDataSource>(config)
                .AddConfigurationRepos<ConfigurationDataSource>();

            return services;
        }
    }
}
