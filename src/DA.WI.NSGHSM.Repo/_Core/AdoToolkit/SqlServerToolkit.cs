using DA.DB.Utils;
using DA.DB.Utils.Entities;
using System.Data;
using System.Transactions;
using static DA.WI.NSGHSM.Core.App.Configuration;

namespace DA.WI.NSGHSM.Repo._Core.AdoToolkit
{
    internal class SqlServerToolkit<TDataSource> : IAdoToolkit
        where TDataSource : DataSource
    {
        private readonly string databaseName;

        private readonly IDbClient ctx;
        private readonly IDbClient initctx;

        public SqlServerToolkit(DataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();

            // I save the Database name and I erase it from DbConnectionInfo
            // to deny the DbClient to create a connection to a Database that could not exist
            DbConnectionInfo initdbInfo = new DbConnectionInfo() { ConnectionString = dataSource.Config.ConnectionString };
            this.databaseName = initdbInfo.Database;
            initdbInfo.Database = null;

            DataSourceConfig initDataSourceConfig = new DataSourceConfig() { ConnectionString = initdbInfo.ConnectionString };
            InitDataSource initDataSource = new InitDataSource();
            initDataSource.SetConfig(initDataSourceConfig);
            this.initctx = initDataSource.GetDbClient();
        }

        public void CreateDatabase()
        {
            initctx.ExecuteNonQuery($"CREATE DATABASE {databaseName} ;");
        }

        public void DeleteDatabase()
        {
            initctx.ExecuteNonQuery($"ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            initctx.ExecuteNonQuery($"DROP DATABASE {databaseName} ;");
        }

        public bool IsDatabaseExisting()
        {
            try
            {

                object result = this.initctx.GetScalar("SELECT database_id FROM sys.databases WHERE Name = :Database",
                                                              ctx.CreateParameter("Database", databaseName));

                int databaseId = 0;

                if (result != null)
                {
                    int.TryParse(result.ToString(), out databaseId);
                }

                return (databaseId > 0) == true;


            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public string GetDatabaseName()
        {
            return databaseName;
        }

        public string GetLastInsertedIdStatement(string tableName)
        {
            return $"SELECT MAX(Id) FROM {tableName} ";
        }


        public void InitializeDatabase(string scriptToExcecute)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                this.ctx.ExecuteNonQuery(scriptToExcecute);

                ts.Complete();
            }
        }

    }

    class InitDataSource : DataSource
    {
        public override string Name => "Init";
    }
}