using Microsoft.Extensions.DependencyInjection;

namespace DA.WI.NSGHSM.Logic.Messaging
{
    public static class ServiceConfig
    {
        internal static IServiceCollection AddMessagingLogic(this IServiceCollection services)
        {
            services.AddSingleton<MessagingLogic>();

            return services;
        }
    }
}