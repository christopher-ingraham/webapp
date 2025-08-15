using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DA.WI.NSGHSM.Core.Services
{
    public class StartupManager
    {
        private readonly IEnumerable<IStartupTask> startupTasks;
        private readonly ILogger<StartupManager> logger;

        public StartupManager(IEnumerable<IStartupTask> startupTasks, ILogger<StartupManager> logger)
        {
            this.startupTasks = startupTasks;
            this.logger = logger;
        }

        public void Execute()
        {
            startupTasks
                .ToList()
                .ForEach(_ => ExecuteStartupTaskSafely(_));
        }

        private void ExecuteStartupTaskSafely(IStartupTask startupTask)
        {
            var startupTaskName = startupTask.ToString();

            try
            {
                logger.LogInformation($"StartupTask [{startupTaskName}] started");

                startupTask.Execute();

                logger.LogInformation($"StartupTask [{startupTaskName}] finished with success");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"StartupTask [{startupTaskName}] finished with error");
            }
        }
    }
}
