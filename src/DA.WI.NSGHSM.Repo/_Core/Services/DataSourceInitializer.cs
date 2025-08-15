using DA.DB.Utils;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Core.Services;
using DA.WI.NSGHSM.Repo._Core.AdoToolkit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Transactions;

namespace DA.WI.NSGHSM.Repo._Core.Services
{
    internal class DataSourceInitializer<TDataSource> : IStartupTask
         where TDataSource : DataSource, new()
    {
        private readonly TDataSource dataSource;
        private readonly ILogger<DataSourceInitializer<TDataSource>> logger;
        private readonly IAdoToolkit toolkit;
        private IDbClient ctx;

        public DataSourceInitializer(
            IOptions<TDataSource> dataSourceOptions,
            ILogger<DataSourceInitializer<TDataSource>> logger)
        {
            this.dataSource = dataSourceOptions.Value;
            dataSource.SetConfig(dataSource.Config); // TODO: register in the right point.
            this.logger = logger;
            this.toolkit = AdoToolkitFactory.Create(dataSource);
            this.ctx = dataSource.GetDbClient();
        }

        public void Execute()
        {
            var initMode = GetInitMode();
            var dataSourceName = dataSource.Name;

            logger.LogInformation($"Initializing with [InitMode]=[{initMode}]...");
            switch (initMode)
            {
                case Core.App.Configuration.DataSourceConfig.InitModeType.Never:
                    logger.LogInformation("Initialization is always skipped.");
                    break;

                case Core.App.Configuration.DataSourceConfig.InitModeType.IfNotExists:
                    InitializeDatabaseIfNotExists();
                    break;

                case Core.App.Configuration.DataSourceConfig.InitModeType.Always:
                    AlwaysInitializeDatabase();
                    break;

                default:
                    throw new NotImplementedException($"[InitMode]=[{initMode}] not implemented.");
            }
            logger.LogInformation($"Successfully initialized.");
        }

        private void InitializeDatabaseIfNotExists()
        {
            if (toolkit.IsDatabaseExisting() == true)
            {
                logger.LogInformation("Database already exists so initialization is skipped.");
                return;
            }

            toolkit.CreateDatabase();
            InitializeDatabase();
        }

        private void AlwaysInitializeDatabase()
        {
            if (toolkit.IsDatabaseExisting() == true)
            {
                logger.LogWarning("Database already exists so database will be deleted and re-created.");
                toolkit.DeleteDatabase();
            }

            toolkit.CreateDatabase();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            logger.LogInformation("Populating database...");

            toolkit.InitializeDatabase(ReadInitScript());

            logger.LogInformation($"...database populated.");
        }

        private Core.App.Configuration.DataSourceConfig.InitModeType GetInitMode()
        {
            return dataSource.Config.InitMode;
        }

        private string ReadInitScript()
        {
            string initScriptPath = GetInitScriptPath();

            if (File.Exists(initScriptPath) == false)
            {
                throw new FileNotFoundException($"Initialization Script file not found at: {Path.GetFullPath(initScriptPath)}");
            }

            return File.ReadAllText(initScriptPath);
        }

        private string GetInitScriptPath()
        {
            var initScriptPath = dataSource.Config.InitScriptPath;

            if (initScriptPath.IsNullOrEmpty() == true)
            {
                return CreateDefaultInitScriptPath();
            }

            return initScriptPath;
        }

        private string CreateDefaultInitScriptPath()
        {
            // format examples:
            //  DataSources/Default/sqlite.default.init.sql
            //  DataSources/Configuration/oracle.configuration.init.sql

            var dataSourceName = dataSource.Name.ToLower();
            var providerName = dataSource.Config.Provider.ToString().ToLower();

            return Path.Combine(
                "DataSources",
                dataSource.Name,
                $"{providerName}.{dataSourceName}.init.sql"
                );
        }

        public override string ToString()
        {
            return $"DataSourceInitializer<{dataSource.GetType().Name}>";
        }
    }
}
