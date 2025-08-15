using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA.WI.NSGHSM.Core.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DA.WI.NSGHSM.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {

        [HttpGet]
        [EnableCors("MyPolicy")]
        [ProducesResponseType(typeof(Configuration), StatusCodes.Status200OK)]
        public IActionResult Get([FromServices]IConfiguration configService)
        {
            var configuration = new Configuration();
            configService.Bind(nameof(Configuration), configuration);
            var endpointsConfiguration = new EndPointsConfiguration();
            configService.Bind("Endpoints", endpointsConfiguration);

            // URL Base Placeholders replacement
            configuration.ApiUrl = InitHelper.EndPointsBaseURLRelpacer(endpointsConfiguration.ApiUrl, configuration.ApiUrl, "[ApiUrl]");
            configuration.StsUrl = InitHelper.EndPointsBaseURLRelpacer(endpointsConfiguration.IdentityServerUrl, configuration.StsUrl, "[IdentityServerUrl]");

            return Ok(configuration);
        }

        public class Configuration
        {
            public string ApiUrl { get; set; }
            public string StsUrl { get; set; }
            public StsClientConfig StsClient { get; set; }

            public class StsClientConfig
            {
                public string ClientId { get; set; }
                public string RedirectUrl { get; set; }
                public string PostLogoutRedirectUri { get; set; }
                public string ResponseType { get; set; }
                public string Scope { get; set; }
                public bool SilentRenew { get; set; }
                public string SilentRenewUrl { get; set; }
                public string OAuthTokenValidationTimeOut { get; set; }
            }

        }

        public class EndPointsConfiguration {
            public string ApiUrl { get; set; }
            public string IdentityServerUrl { get; set; }
        }

    }
}