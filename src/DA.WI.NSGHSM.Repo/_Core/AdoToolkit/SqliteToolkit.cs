using DA.DB.Utils;
using DA.DB.Utils.Entities;
using System.IO;
using System.Transactions;

namespace DA.WI.NSGHSM.Repo._Core.AdoToolkit
{
    internal class SqliteToolkit<TDataSource> : IAdoToolkit
        where TDataSource : DataSource
    {
        private readonly IDbClient ctx;

        public SqliteToolkit(DataSource dataSource)
        {
            //this.ctx = ctx;
            //ctx.Initialize(new DbConnectionInfo() { ConnectionString = dataSource.Config.ConnectionString });
            this.ctx = dataSource.GetDbClient();
        }

        public void CreateDatabase()
        {
            CreateDbFolderIfNotExisting();
 
            // just open a connection to let sqlite create automatically the database
            ctx.GetDataReader("SELECT sqlite_version()");
        }

        public void CreateDbFolderIfNotExisting()
        {
            var databaseName = GetDatabaseName();
            var folderPath = Path.GetDirectoryName(databaseName);

            if (Directory.Exists(folderPath) == false)
                Directory.CreateDirectory(folderPath);
        }

        public void DeleteDatabase()
        {
            var databaseName = GetDatabaseName();
            File.Delete(databaseName);
        }

        public bool IsDatabaseExisting()
        {
            var databaseName = GetDatabaseName();
            var dbExists = File.Exists(databaseName);

            return dbExists;
        }

        public string GetDatabaseName()
        {
            var DataSourceInfoStart = ctx.DbConnectionInfo.ExplicitConnectionString.IndexOf("Data Source=") + "Data Source=".Length;
            var dbPath = ctx.DbConnectionInfo.ExplicitConnectionString.Substring(DataSourceInfoStart);
            var DataSourceInfoEnd = dbPath.IndexOf(";");
            dbPath = dbPath.Substring(0, DataSourceInfoEnd);
            return dbPath;
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
}