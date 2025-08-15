using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Report
{

    public class StoppageReportBaseDto
    {
    }

    public class StoppageReportDto : StoppageReportBaseDto
    {
    }

    public class StoppageReportListItemDto : StoppageReportDto
    {
        public DateTime StartDelay { get; set; }
        public DateTime FilterDate { get; set; }
        public int StpCounter { get; set; }
        public string Duration_HHMM { get; set; }
        public string DelayTime { get; set; }
        public string StpGroupLabel { get; set; }
        public string StpReasonLabel { get; set; }
        public int ShiftId { get; set; }
        public string ProductionShiftLabel { get; set; }
        public string CrewId { get; set; }
    }

    public class StoppageReportForInsertDto : StoppageReportBaseDto
    {
    }

    public class StoppageReportForUpdateDto : StoppageReportBaseDto
    {
    }

    public class StoppageReportDetailDto : StoppageReportDto
    {
    }

     public class StoppageReportListFilterDto 
    {
        public DateTimeOffset? SearchEndDelayDateFrom { get; set; }
        public DateTimeOffset? SearchEndDelayDateTo { get; set; }
        public int? SearchShiftId { get; set; }

    }
}