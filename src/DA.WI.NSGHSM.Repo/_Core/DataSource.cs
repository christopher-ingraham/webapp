using DA.DB.Utils;
using DA.DB.Utils.Entities;
using DA.WI.NSGHSM.Core.App;
using System;
using System.IO;

namespace DA.WI.NSGHSM.Repo._Core
{
    public abstract class DataSource : IDisposable  
    {
        private IDbClient dbClient;

        public abstract string Name { get; }

        public Core.App.Configuration.DataSourceConfig Config { get; set; }

        public void Dispose()
        {
            if(this.dbClient != null)
            {
                this.dbClient.Dispose();
            }
        }

        public IDbClient GetDbClient()
        {
            return dbClient;
        }

        internal void SetConfig(Core.App.Configuration.DataSourceConfig dataSourceConfig)
        {
            Config = dataSourceConfig;
            dbClient = new DbClient();
            dbClient.Initialize(new DbConnectionInfo() { ConnectionString = Config?.ConnectionString });
        }
    }
}
