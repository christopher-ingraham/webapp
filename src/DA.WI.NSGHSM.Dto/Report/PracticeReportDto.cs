using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Report
{

    public class PracticeReportBaseDto
    {
    }

    public class PracticeReportDto : PracticeReportBaseDto
    {
    }

    public class PracticeReportListItemDto : PracticeReportDto
    {
        public string PracticeId { get; set; }
        public int MillMode { get; set; }
        public string CenterId { get; set; }
        public string MillModeLabel { get; set; }
        public string MaterialGradeId { get; set; }
        public int GradeGroupId { get; set; }
        public string GradeGroupLabel { get; set; }
        public double Density { get; set; }
        public double EntryThk { get; set; }
        public double TargetThk { get; set; }
        public double TargetColdThk { get; set; }
        public double EntryWdt { get; set; }
        public double TargetWdt { get; set; }
        public double TargetColdWdt { get; set; }
        public double PieceLength { get; set; }
        public double EntryTemp { get; set; }
        public int nDisabledStand { get; set; }
        public string Operator { get; set; }
        public DateTime Revision { get; set; }
        public DateTime FilterDate { get; set; }
    }

    public class PracticeReportForInsertDto : PracticeReportBaseDto
    {
    }

    public class PracticeReportForUpdateDto : PracticeReportBaseDto
    {
    }

    public class PracticeReportDetailDto : PracticeReportDto
    {
    }

     public class PracticeReportListFilterDto 
    {
        public string SearchCenterId { get; set; }
        public string SearchPracticeId { get; set; }
        public int SearchMillMode { get; set; }
        public string SearchMaterialGradeId { get; set; }
        public int? SearchEntryThicknessFrom { get; set; }
        public int? SearchEntryThicknessTo { get; set; }
        public DateTimeOffset? SearchRevisionDateFrom { get; set; }
        public DateTimeOffset? SearchRevisionDateTo { get; set; }

    }
}