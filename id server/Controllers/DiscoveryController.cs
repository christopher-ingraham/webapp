using Microsoft.AspNetCore.Mvc;

namespace DA.WI.NSGHSM.IdentityServer.Controllers
{
    [ApiController]
    [Route(".well-known")]
    public class DiscoveryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DiscoveryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("openid-configuration")]
        public ActionResult GetConfiguration()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var issuer = _configuration.GetValue<string>("IssuerUri") ?? baseUrl;

            var configuration = new
            {
                issuer = issuer,
                authorization_endpoint = $"{baseUrl}/connect/authorize",
                token_endpoint = $"{baseUrl}/connect/token",
                userinfo_endpoint = $"{baseUrl}/connect/userinfo",
                end_session_endpoint = $"{baseUrl}/connect/endsession",
                jwks_uri = $"{baseUrl}/.well-known/jwks",
                scopes_supported = new[] { "openid", "profile", "email", "NSGHSM-api" },
                response_types_supported = new[] { "code", "token", "id_token", "code id_token", "code token", "id_token token", "code id_token token" },
                response_modes_supported = new[] { "query", "fragment", "form_post" },
                grant_types_supported = new[] { "authorization_code", "implicit", "password", "client_credentials", "refresh_token" },
                subject_types_supported = new[] { "public" },
                id_token_signing_alg_values_supported = new[] { "RS256" },
                token_endpoint_auth_methods_supported = new[] { "client_secret_basic", "client_secret_post" },
                claims_supported = new[] { "sub", "name", "given_name", "family_name", "email", "role", "department" },
                code_challenge_methods_supported = new[] { "plain", "S256" }
            };

            return Ok(configuration);
        }

        [HttpGet("jwks")]
        public ActionResult GetJwks()
        {
            // For development - return empty JWKS
            // In production, you would return the actual public keys
            var jwks = new
            {
                keys = new object[] { }
            };

            return Ok(jwks);
        }
    }
}
