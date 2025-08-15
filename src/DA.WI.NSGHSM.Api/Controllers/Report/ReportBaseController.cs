using DA.WI.NSGHSM.Dto.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;

namespace DA.WI.NSGHSM.Api.Controllers.Report
{
    public class ReportGeneratorConfiguration {

        private IConfiguration configuration;

        public string CreatorFolder { get; private set; }
        public string CreatorApplication { get; private set; }
        public string TemplateFolder { get; private set; }
        public string OutputFolder { get; private set; }

        public string FullCreatorApplication {
            get { return Path.GetFullPath(Path.Combine(CreatorFolder, CreatorApplication)); }
        }

        public ReportGeneratorConfiguration(IConfiguration iConfiguration) {
            this.configuration = iConfiguration;
            var ReportGenerator = iConfiguration.GetSection("ReportGenerator");
            string unsafeCreatorFolder = ReportGenerator.GetValue<string>("CreatorFolder");
            CreatorFolder = (string.IsNullOrEmpty(unsafeCreatorFolder)) ? Directory.GetCurrentDirectory() : Path.GetFullPath(unsafeCreatorFolder);            
            CreatorApplication = ReportGenerator.GetValue<string>("CreatorApplication");
            TemplateFolder = ReportGenerator.GetValue<string>("TemplateFolder");
            OutputFolder = ReportGenerator.GetValue<string>("OutputFolder");
        }
    }

    public abstract class ReportBaseController<T> : ControllerBase {
        ReportGeneratorConfiguration configuration { get; set; }
        ILogger<T> log { get; set; }

        public ReportBaseController(IConfiguration iConfiguration, ILogger<T> logger) {
            configuration = new ReportGeneratorConfiguration(iConfiguration);
            log = logger;
        }

        [HttpGet("report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReport([FromQuery] GenerateReportRequestDto reportRequest) {
            log.LogDebug("GenerateReportRequestDto {0}", reportRequest.ToString());
            if (reportRequest.isValid) {
                log.LogDebug("Request is VALID!");
                string localFileName = await CreateReport(reportRequest);
                if (string.IsNullOrEmpty(localFileName)) {
                    // For whatever reason CreateReport failed,
                    // do not disclose our side failures to client.
                    // Just tell them we couldn't find it.
                    // Check logs to find out what happened.
                    return NotFound();
                } else {
                    string publicFileName = GeneratePublicFileName(reportRequest);
                    return DownloadReport(localFileName, publicFileName);
                }
            } else {
                log.LogDebug("Request is NOT valid!");
                return BadRequest();
            }            
        }

        private Task<string> CreateReport(GenerateReportRequestDto reportRequest) {
            string outputFileName = string.Format("{0}.xlsx", Guid.NewGuid());
            string commandParameters = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" {6}",
                /* 0 fixed */ configuration.CreatorFolder,
                /* 1 fixed */ reportRequest.ReportLanguage,
                /* 2 fixed */ configuration.TemplateFolder,
                /* 3 fixed */ configuration.OutputFolder,
                /* 4 fixed */ outputFileName,
                /* 5 fixed */ (int) reportRequest.ReportType,
                /* 6 param */ reportRequest.ReportParams);

            StringBuilder contextText = new StringBuilder();
            contextText.AppendLine("---");
            contextText.AppendLine(string.Format("Directory.GetCurrentWD = [{0}]", Directory.GetCurrentDirectory()));
            contextText.AppendLine(string.Format("         CreatorFolder = [{0}]", configuration.CreatorFolder));
            contextText.AppendLine(string.Format("        ReportLanguage = [{0}]", reportRequest.ReportLanguage));
            contextText.AppendLine(string.Format("        TemplateFolder = [{0}]", configuration.TemplateFolder));
            contextText.AppendLine(string.Format("          OutputFolder = [{0}]", configuration.OutputFolder));
            contextText.AppendLine(string.Format("        outputFileName = [{0}]", outputFileName));
            contextText.AppendLine(string.Format("FullCreatorApplication = [{0}]", configuration.FullCreatorApplication));
            contextText.AppendLine(string.Format("     commandParameters = [{0}]", commandParameters));
            contextText.AppendLine("===");
            log.LogDebug(contextText.ToString());
            
            var t = new TaskCompletionSource<string>();

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo {
                Arguments = commandParameters,
                ErrorDialog = false,
                FileName = configuration.FullCreatorApplication,
                UseShellExecute = false,
                WorkingDirectory = configuration.CreatorFolder,
            };
            p.EnableRaisingEvents = true;
            p.Exited += (object sender, EventArgs e) => {
                Process exitedProcess = sender as Process;
                string ret = Path.GetFullPath(Path.Combine(configuration.CreatorFolder, configuration.OutputFolder, outputFileName));

                switch (exitedProcess.ExitCode) {
                    case 0:                        
                        t.SetResult(ret);
                        break;
                    case 571:
                        t.SetException(new ApplicationException("error 571"));
                        break;
                    default:
                        t.SetException(new ApplicationException(string.Format("unexpected {0} from report creator", exitedProcess.ExitCode)));
                        break;
                }
            };            

            try {
                p.Start();
            }
            catch (Exception startException) {
                t.SetException(startException);
            }

            return t.Task;
        }
        private string GeneratePublicFileName(GenerateReportRequestDto reportRequest) {
            DateTime now = DateTime.Now;
            string timestamp = string.Format("{0}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}",
                now.Year, now.Month, now.Day,
                now.Hour, now.Minute, now.Second, now.Millisecond);
            string paramList = string.Join("+", reportRequest.ReportParamList);
            return string.Format("{0}_Report__T({1})_P({2})__{3}.xlsx",
                ControllerContext.ActionDescriptor.ControllerName,
                reportRequest.ReportType, paramList, timestamp);
        }
        private FileContentResult DownloadReport(string localFileName, string publicFileName) {
            byte[] fileBytes = System.IO.File.ReadAllBytes(localFileName);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(fileBytes, contentType, publicFileName);
        }

    }
}