using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.DirectoryServices.AccountManagement;

namespace DA.WI.DirectoryServices
{
    public class PrincipalContextManager
    {
        private readonly ILogger<PrincipalContextManager> logger;
        private readonly PrincipalContextOptions<PrincipalContextManager> options;

        public bool IsEnabled => (options.IsEnabled == true);

        public PrincipalContextManager(IOptions<PrincipalContextOptions<PrincipalContextManager>> options, ILogger<PrincipalContextManager> logger)
        {
            this.logger = logger;
            this.options = options.Value;
        }

        private PrincipalContext InitializePrincipalContext()
        {

            PrincipalContext principalContext = null;
 
            try
            {

                principalContext = (IsConfiguredForCurrentDomain() == true)
                    ? InitializePrincipalContextForCurrentDomain()
                    : InitializePrincipalContextForExternalDomain();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw (ex);
            }

            logger.LogDebug("PrincipalContext successfully initialized");

            return principalContext;
        }

        private bool IsConfiguredForCurrentDomain() => (String.IsNullOrWhiteSpace(options.Domain) == true);

        private PrincipalContext InitializePrincipalContextForCurrentDomain()
        {
            logger.LogDebug("Initializing PrincipalContext on current Domain...");

            return new PrincipalContext(ContextType.Domain);
        }

        private PrincipalContext InitializePrincipalContextForExternalDomain()
        {
            logger.LogDebug($"Initializing PrincipalContext on external Domain [{options.Domain}] using service user [{options.ServiceUserName}]...");

            return new PrincipalContext(
                ContextType.Domain,
                options.Domain,
                options.ServiceUserName,
                options.ServiceUserPassword);
        }

        public bool ValidateCredential(string userName, string password)
        {
            logger.LogInformation($"Validating: {userName}; **** ");

            if(options.IsEnabled == false)
            {
                this.logger.LogWarning($"User {userName} is always valid because Domain validation is disabled. This ValidateCredential call is unnecessary.");
                return true;
            }

            bool isUserValid = false;
            using (var principalContext = InitializePrincipalContext())
            {
                isUserValid = principalContext.ValidateCredentials(userName, password);
            }

            logger.LogInformation((isUserValid == true) ? $"User {userName} is valid" : $"User {userName} is not valid");

            return (isUserValid == true);
        }
    }

}
