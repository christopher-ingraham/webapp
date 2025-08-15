using DA.DB.Utils;
using System;

namespace DA.WI.NSGHSM.Repo._Core.AdoToolkit
{
    internal static class AdoToolkitFactory
    {
        internal static IAdoToolkit Create<TDataSource>(TDataSource dataSource)
            where TDataSource: DataSource
        {
            var provider = dataSource.Config.Provider;
            switch (provider)
            {
                case Core.App.Configuration.DataSourceConfig.ProviderType.Sqlite:
                    return new SqliteToolkit<TDataSource>(dataSource);

                case Core.App.Configuration.DataSourceConfig.ProviderType.SqlServer:
                    return new SqlServerToolkit<TDataSource>(dataSource);

                default:
                    throw new NotImplementedException($"Toolkit for provider [{provider}] is not implemented yet!");
            }
        }
    }
}
