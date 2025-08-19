using Microsoft.AspNetCore.Mvc;

namespace DA.WI.NSGHSM.IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet("Login")]
        public ActionResult Login(
            [FromQuery] string client_id,
            [FromQuery] string redirect_uri,
            [FromQuery] string response_type,
            [FromQuery] string scope,
            [FromQuery] string state = "")
        {
            // Return a simple login form or redirect to your SPA login page
            var loginPageUrl = $"/login?client_id={client_id}&redirect_uri={Uri.EscapeDataString(redirect_uri)}&response_type={response_type}&scope={Uri.EscapeDataString(scope)}&state={state}";
            return Redirect(loginPageUrl);
        }
    }
}
