using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Core.App;
using DA.WI.NSGHSM.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace DA.WI.NSGHSM.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class LocaleController : ControllerBase
    {
        private const string LocaleFolder = @"Locales";
        private Configuration _configuration;

        public LocaleController(Configuration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{localeCode}")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public IActionResult Get(string localeCode)
        {
            string jsonFileContent = GetContentFromFile(localeCode);

            if (jsonFileContent == null)
            {
                throw new NotFoundException("Locales", localeCode);
            }

            return Ok(jsonFileContent);
        }

        private string GetContentFromFile(string code)
        {
            return System.IO.File.ReadAllText(Path.Combine(LocaleRootPath, $"{code}.json"));
        }

        private string LocaleRootPath => Path.Combine(this._configuration.RootFolder, LocaleFolder);
    }
}