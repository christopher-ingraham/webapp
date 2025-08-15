using DA.WI.NSGHSM.Core.Services;
using DA.WI.NSGHSM.Core.Test._Mockups;
using DA.WI.NSGHSM.XUnitExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DA.WI.NSGHSM.Core.Test.Services
{
    public class StartupManagerTest
    {
        [FactWithAutomaticDisplayName]
        public void Execute_EmptyStartupTasks()
        {
            var startupTasks = new IStartupTask[] { };
            var logger = new LoggerMock<StartupManager>();

            var sut = new StartupManager(startupTasks, logger);

            sut.Execute();

            Assert.True(true);
        }

        [FactWithAutomaticDisplayName]
        public void Execute_OneStartupTask()
        {
            var startupTaskExecuted = false;
            var startupTasks = new IStartupTask[]
            {
                new MockStartupTask { ExecuteCallback = () => { startupTaskExecuted = true; } }
            };
            var logger = new LoggerMock<StartupManager>();

            var sut = new StartupManager(startupTasks, logger);
            sut.Execute();

            Assert.True(startupTaskExecuted == true);
        }


        [FactWithAutomaticDisplayName]
        public void Execute_OneStartupTaskThrowingException_CatchedAndLogged()
        {
            var errorLogged = false;
            var startupTasks = new IStartupTask[]
            {
                new MockStartupTask { ExecuteCallback = () => { throw new Exception(); } }
            };
            var logger = new LoggerMock<StartupManager>()
            {
                LogCallback = (logLevel, eventId, state, exception) =>
                {
                    errorLogged = (logLevel == Microsoft.Extensions.Logging.LogLevel.Error);
                }
            };

            var sut = new StartupManager(startupTasks, logger);
            sut.Execute();

            Assert.True(errorLogged == true);
        }

        [FactWithAutomaticDisplayName]
        public void Execute_TwoStartupTasks_FirstThrowingException_SecondExecuted()
        {
            var secondTaskExecuted = false;
            var startupTasks = new IStartupTask[]
            {
                new MockStartupTask { ExecuteCallback = () => { throw new Exception(); } },
                new MockStartupTask { ExecuteCallback = () => { secondTaskExecuted = true; } }
            };
            var logger = new LoggerMock<StartupManager>();

            var sut = new StartupManager(startupTasks, logger);
            sut.Execute();

            Assert.True(secondTaskExecuted == true);
        }

        private class MockStartupTask : IStartupTask
        {
            public Action ExecuteCallback { get; set; }
            public void Execute()
            {
                ExecuteCallback?.Invoke();
            }
        }
    }
}
