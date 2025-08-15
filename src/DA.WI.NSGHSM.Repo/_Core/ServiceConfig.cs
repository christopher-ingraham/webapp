using DA.WI.NSGHSM.Core.Services;
using DA.WI.NSGHSM.IRepo._Core.Configuration;
using DA.WI.NSGHSM.Repo._Core.Configuration;
using DA.WI.NSGHSM.Repo._Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Repo._Core
{
    internal static class ServiceConfig
    {


        internal static IServiceCollection AddRepo<TIRepo, TRepo>(this IServiceCollection services)
    where TIRepo : class
    where TRepo : class, TIRepo
        {
            return services.AddTransient<TIRepo, TRepo>();
        }

        internal static IServiceCollection AddDataSource<TDataSource>(this IServiceCollection services, Core.App.Configuration config)
    where TDataSource : DataSource, new()
        {
            var dataSource = new TDataSource();
            dataSource.SetConfig(config.DataSources[dataSource.Name]);

            return services
                .ConfigureDataSourceOptions(dataSource)
                .AddDataSource(dataSource)
                .AddDataSourceInitializer<TDataSource>()
                ;
        }

        private static IServiceCollection ConfigureDataSourceOptions<TDataSource>(this IServiceCollection services, TDataSource dataSource)
    where TDataSource : DataSource, new()
        {
            return services
                .Configure<TDataSource>(opt =>
                {
                    opt.Config = dataSource.Config;
                });
        }

        private static IServiceCollection AddDataSource<TDataSource>(this IServiceCollection services, TDataSource dataSource)
            where TDataSource : DataSource, new()
        {
            return services
                .AddScoped<TDataSource>((_) =>
                {
                    var dataSourceTemp = new TDataSource();
                    dataSourceTemp.SetConfig(dataSource.Config);
                    return dataSourceTemp;
                });
        }



        private static IServiceCollection AddDataSourceInitializer<TDataSource>(this IServiceCollection services)
            where TDataSource : DataSource, new()
        {
            return services
                .AddTransient<IStartupTask, DataSourceInitializer<TDataSource>>();
        }

        internal static IServiceCollection AddConfigurationRepos<TDataSource>(this IServiceCollection services)
            where TDataSource : DataSource, new()
        {
            return services.AddRepo<IUserRepo<TDataSource>, UserRepo<TDataSource>>();
        }
    }
}
