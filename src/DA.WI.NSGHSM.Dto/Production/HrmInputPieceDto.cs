using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class HrmInputPieceBaseDto
    {
        public int PieceNo { get; set; }
        public int JobPieceSeq { get; set; }
        public string PieceId { get; set; }
        public string JobId { get; set; }
        public int PassNo { get; set; }
        public int HeatNo { get; set; }
        public int HeatSeq { get; set; }
        public int HeatPieceSeq { get; set; }
        public int Transition { get; set; }
        public int SourceCodeId { get; set; }
        public int DestCodeId { get; set; }
        public string BaseGradeId { get; set; }
        public int UseBaseGrade { get; set; }
        public string PreliminaryMatGradeId { get; set; }
        public string MaterialGradeId { get; set; }
        public string MaterialSpecId { get; set; }
        public double PreliminaryThk { get; set; } 
        public double PreliminaryThkHead { get; set; }
        public double PreliminaryThkTail { get; set; }
        public double PreliminaryWdt { get; set; }
        public double PreliminaryWdtHead { get; set; }
        public double PreliminaryWdtTail { get; set; }
        public double PreliminaryWdtChg { get; set; }
        public double PreliminaryLen { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
        public double EntryTemp { get; set; }
        public double FurnaceDischargeTemp { get; set; }
        public double TargetWidth { get; set; }
        public double TargetWidthPtol { get; set; }
        public double TargetWidthNtol { get; set; }
        public double TargetThickness { get; set; }
        public double TargetThicknessPtol { get; set; }
        public double TargetThicknessNtol { get; set; }
        public double TargetWeight { get; set; }
        public double TargetWeightPtol { get; set; }
        public double TargetWeightNtol { get; set; }
        public double TargetExitTemp { get; set; }
        public double TargetExitTempPtol { get; set; }
        public double TargetExitTempNtol { get; set; }
        public double TargetTempFmCustomertol { get; set; }
        public double TargetTempDC { get; set; }
        public double TargetTempDCPtol { get; set; }
        public double TargetTempDCNtol { get; set; }
        public double TargetTempDCCustomertol { get; set; }
        public double TargetProfile { get; set; }
        public double TargetProfilePtol { get; set; }
        public double TargetProfileNtol { get; set; }
        public double TargetProfileCustomertol { get; set; }
        public double TargetFlatness { get; set; }
        public double TargetFlatnessPtol { get; set; }
        public double TargetFlatnessNtol { get; set; }
        public double TargetFlatnessCustomertol { get; set; }
        public double TargetInternalDiameter { get; set; }
        public int UseMeasWidth { get; set; }
        public double MeasuredWidthHead { get; set; }
        public double MeasuredWidthTail { get; set; }
        public int UseMeasTemp { get; set; }
        public double MeasuredTemp { get; set; }
        public DateTime CreationDatetime { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public string Operator { get; set; }
        public DateTime Revision { get; set; }
        public string OrderNumber { get; set; }
        public string OrderPosition { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContactName { get; set; }
        public int EnduseSurfaceRating { get; set; }
        public string AreaType { get; set; }
        public int TrialFlag { get; set; }
        public string TrialNo { get; set; }
        public string CarrierMode { get; set; }
        public string EndUse { get; set; }
        public int PieceSeq { get; set; }
        public int GradeGroupId { get; set; }
        public string HeatId { get; set; }
        public double ThicknessHead { get; set; }
        public double ThicknessTail { get; set; }
        public double Width { get; set; }
        public double WidthHead { get; set; }
        public double WidthTail { get; set; }
        public double WidthChange { get; set; }
        public string CustomerName { get; set; }
    }

    public class HrmInputPieceDto : HrmInputPieceBaseDto
    {

    }

    public class HrmInputPieceListItemDto : HrmInputPieceDto
    {

        public string TransitionLabel { get; set; }
        public string TrialFlagLabel { get; set; }
        public string StatusLabel { get; set; }

    }

    public class HrmInputPieceForInsertDto : HrmInputPieceBaseDto
    {


    }

    public class HrmInputPieceForUpdateDto : HrmInputPieceBaseDto
    {
 
    }

    public class HrmInputPieceDetailDto : HrmInputPieceDto
    {
        public string AlloySpecCode { get; set; }
        public int AlloySpecVersion { get; set; }
        public int AlloySpecCnt { get; set; }
    }

    public class HrmInputPieceLookupDto
    {
        public string Display { get; set; }
        public int Value { get; set; }
    }

    public class HrmInputPieceListFilterDto
    {
        public string SearchSlabNo { get; set; }
        public string SearchHeatNo { get; set; }
        public string SearchStringNo { get; set; }
        public string SearchCustomerOrderNo { get; set; }
        public string SearchCustomerName { get; set; }
        public int? SearchPieceStatus { get; set; }
        public int? SearchProductionStatusFrom { get; set; }
        public int? SearchProductionStatusTo { get; set; }
        public DateTimeOffset? SearchCreationTimeFrom { get; set; }
        public DateTimeOffset? SearchCreationTimeTo { get; set; }

    }
}