using DA.WI.NSGHSM.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<TStartupTask>(this IServiceCollection services)
            where TStartupTask : class, IStartupTask
        {
            if (services == null)
                return null;

            return services.AddTransient<IStartupTask, TStartupTask>();
        }
    }
}
