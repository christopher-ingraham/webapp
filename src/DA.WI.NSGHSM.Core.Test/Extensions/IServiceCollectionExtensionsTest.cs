using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Core.Services;
using DA.WI.NSGHSM.XUnitExtensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace DA.WI.NSGHSM.Core.Test.Extensions
{
    public class IServiceCollectionExtensionsTest
    {
        [FactWithAutomaticDisplayName]
        public void ServiceCollection_Null_AddStartupTask_DoesntThrowsForNullObjectReference()
        {
            IServiceCollection sut = null;

            var result = sut.AddStartupTask<StartupTaskMock>();

            Assert.True(true);
        }

        [FactWithAutomaticDisplayName]
        public void ServiceCollection_AddStartupTask_RegisteredAsTransient()
        {
            IServiceCollection sut = new ServiceCollection();

            sut.AddStartupTask<StartupTaskMock>();

            var result = new ServiceDescriptor[1];
            sut.CopyTo(result, 0);
            var expectedRegistration = new ServiceDescriptor(typeof(IStartupTask), typeof(StartupTaskMock), ServiceLifetime.Transient);

            Assert.Single(result);
            result.First().Should().BeEquivalentTo(expectedRegistration);
        }

        private class StartupTaskMock : IStartupTask
        {
            public void Execute()
            {
                throw new NotImplementedException();
            }
        }
    }
}
