using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Report
{

    public class CoilGeneralReportBaseDto
    {
    }

    public class CoilGeneralReportDto : CoilGeneralReportBaseDto
    {
    }

    public class CoilGeneralReportListItemDto : CoilGeneralReportDto
    {
        public long OutPieceNo { get; set; }
        public int InPieceNo { get; set; }
        public string OutPieceId { get; set; }
        public string JobId { get; set; }
        public string InPieceId { get; set; }
        public int ProdCoilNum { get; set; }
        public string HeatId { get; set; }
        public string MaterialGradeId { get; set; }
        public string TrialFlag { get; set; }
        public double TargetColdWidth { get; set; }
        public double ExitWidth { get; set; }
        public double TargetColdThickness { get; set; }
        public double ExitThk { get; set; }
        public double CalculatedWeight { get; set; }
        public double MeasuredWeight { get; set; }
        public double ExternalDiameter { get; set; }
        public double Length { get; set; }
        public string CustomerId { get; set; }
        public DateTime ProductionStopDate { get; set; }
        public int ShiftId { get; set; }
        public double EntryThk { get; set; }
        public double TargetThickness { get; set; }
        public double NominalWidth { get; set; }
        public double Weight { get; set; }
        public DateTime ProductionStartDate { get; set; }
        public DateTime FilterDate { get; set; }
        public int Status { get; set; }
        public string ProductionShiftLabel { get; set; }
        public string CrewId { get; set; }
    }

    public class CoilGeneralReportForInsertDto : CoilGeneralReportBaseDto
    {
    }

    public class CoilGeneralReportForUpdateDto : CoilGeneralReportBaseDto
    {
    }

    public class CoilGeneralReportDetailDto : CoilGeneralReportDto
    {
    }

     public class CoilGeneralReportListFilterDto 
    {
        public DateTimeOffset? SearchProductionStopDateFrom { get; set; }
        public DateTimeOffset? SearchProductionStopDateTo { get; set; }
        public string SearchProducedPieceId { get; set; }
        public int? SearchInputSlabNumber { get; set; }
        public string SearchHeatNumber { get; set; }
        public string SearchMaterialGradeId { get; set; }
        public int? SearchCoilStatus { get; set; }
        public int? SearchShiftId { get; set; }
        public int? SearchExitThicknessFrom { get; set; }
        public int? SearchExitThicknessTo { get; set; }
        public int? SearchExitWidthFrom { get; set; }
        public int? SearchExitWidthTo { get; set; }
    }
}