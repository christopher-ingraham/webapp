using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Report
{

    public class ShiftReportBaseDto
    {
    }

    public class ShiftReportDto : ShiftReportBaseDto
    {
    }

    public class ShiftReportListItemDto : ShiftReportDto
    {
        public int ShiftCnt { get; set; }
        public DateTime ShiftStartDate { get; set; }
        public DateTime ShiftEndDate { get; set; }
        public DateTime FilterDate { get; set; }
        public int ShiftId { get; set; }
        public string ProductionShiftLabel { get; set; }
        public string CrewId { get; set; }
        public DateTime CurrentMonthStart { get; set; }
        public DateTime CurrentMonthStop { get; set; }
        public double MtdProduced { get; set; }
        public double TotOutWeight { get; set; }
        public double TotInWeight { get; set; }
        public double OptWeight { get; set; }
        public double Utilization { get; set; }
        public double LastPieceNo { get; set; }
        public int TotOutPiecesNum { get; set; }
    }

    public class ShiftReportForInsertDto : ShiftReportBaseDto
    {
    }

    public class ShiftReportForUpdateDto : ShiftReportBaseDto
    {
    }

    public class ShiftReportDetailDto : ShiftReportDto
    {
    }

     public class ShiftReportListFilterDto 
    {
        public DateTimeOffset? SearchShiftStartDateFrom { get; set; }
        public DateTimeOffset? SearchShiftStartDateTo { get; set; }
        public int? SearchShiftId { get; set; }

    }
}