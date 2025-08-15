using System.Collections.Generic;

namespace DA.WI.NSGHSM.Dto.Report
{
    // SUPPORTED REPORT TYPE CODES
    public enum ReportType
    {
        COIL_PRODUCTION = 0,
        TIME_PRODUCTION = 1,
        SHIFT = 2,
        COBBLE = 3,
        STOPPAGE = 4,
        PRACTICE = 5
    }

    public class GenerateReportRequestDto
    {
        public string ReportLanguage { get; set; }
        public ReportType ReportType { get; set; }
        public string ReportParam1 { get; set; }
        public string ReportParam2 { get; set; }
        public string ReportParam3 { get; set; }
        public string ReportParam4 { get; set; }
        public string ReportParam5 { get; set; }
        public string ReportParam6 { get; set; }
        public string ReportParam7 { get; set; }

        public uint ParamCountByType
        {
            get
            {
                switch (ReportType)
                {
                    case ReportType.COIL_PRODUCTION: return 2;
                    case ReportType.TIME_PRODUCTION: return 4;
                    case ReportType.SHIFT: return 5;
                    case ReportType.COBBLE: return 2;
                    case ReportType.STOPPAGE: return 4;
                    case ReportType.PRACTICE: return 3;
                    default: return 0;
                }
            }
        }
        public bool isValid
        {
            get
            {
                bool isReportLanguageValid = isStringValid(ReportLanguage);
                bool isReportTypeValid = (ReportType >= ReportType.COIL_PRODUCTION) && (ReportType <= ReportType.PRACTICE);

                return (isReportLanguageValid && isReportTypeValid);
            }
        }
        private bool isStringValid(string candidate)
        {
            return !(string.IsNullOrEmpty(candidate) || string.IsNullOrWhiteSpace(candidate));
        }

        public string[] ReportParamList
        {
            get
            {
                uint paramCount = ParamCountByType;
                List<string> paramList = new List<string>();
                string[] candidates = {
                    ReportParam1, ReportParam2, ReportParam3, ReportParam4,
                    ReportParam5, ReportParam6, ReportParam7
                };

                for (uint index = 0; ((index < candidates.Length) && (index < paramCount)); index++)
                {
                    string candidate = candidates[index];
                    // Empty parameters MUST be present: replace with triple dash
                    paramList.Add(isStringValid(candidate) ? candidate : "---");
                }

                return paramList.ToArray();
            }
        }

        public string ReportParams
        {
            get { return "\"" + string.Join("\" \"", ReportParamList) + "\""; }
        }

        public override string ToString()
        {
            return string.Format("{{ ReportParam1=\"{0}\", ReportParam2=\"{1}\", ReportParam3=\"{2}\", ReportParam4=\"{3}\", ReportParam5=\"{4}\", ReportParam6=\"{5}\", ReportParam7=\"{6}\" }}",
                ReportParam1, ReportParam2, ReportParam3, ReportParam4, ReportParam5, ReportParam6, ReportParam7);
        }
    }
}