using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Core.App;
using DA.WI.NSGHSM.Core.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DA.WI.NSGHSM.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class AppUserInfoController : ControllerBase
    {
        private readonly ILogger<AppUserInfoController> logger;

        public AppUserInfoController(ILogger<AppUserInfoController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AppUserInfo), StatusCodes.Status200OK)]
        public IActionResult Get([FromServices]Configuration config, [FromServices]IHostingEnvironment hostEnv)
        {

            var appUserInfo = AppUserInfo.FromConfiguration(config, hostEnv);

            appUserInfo.User = AppUserInfo.UserInfo.FromCurrentUser(User);

            return Ok(appUserInfo);
        }

        public class AppUserInfo
        {
            public static AppUserInfo FromConfiguration(Configuration config, IHostingEnvironment hostEnv)
            {
                var appConfig = config?.Application ?? new Configuration.ApplicationConfig();

                AppUserInfo appUserInfo;
                appUserInfo = new AppUserInfo
                {
                    Application = config?.Application
                };

                return appUserInfo;

            }

            public Configuration.ApplicationConfig Application { get; set; }

            public UserInfo User { get; set; }

            public class UserInfo
            {
                public string UserName { get; set; }

                public string[] Roles { get; set; }

                public static UserInfo FromCurrentUser(ClaimsPrincipal user)
                {
                    return new UserInfo
                    {
                        UserName = user.GetName(),
                        Roles = user.GetRoles().ToArrayOrEmpty()
                    };
                }
            }
        }

    }
}