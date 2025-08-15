using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{
    public class ProducedCoilsBaseDto
    {
        public long OutPieceNo { get; set; }   
        public string OutPieceId { get; set; }  // Produced Coild ID
        public int OutPieceCnt { get; set; }
        public int OutPieceSeq { get; set; }    // Coil Sequence
        public string InPieceId { get; set; }  // Input Slab ID
        public string JobId { get; set; }   // String No
        public double TargetWidth { get; set; }    
        public double TargetThickness { get; set; }    // Target Cold Thickness
        public int EndOfInGotFlag { get; set; }   // Last Ingot Flag
        public int TestCut { get; set; }     // Test Cut
        public int TrialFlag { get; set; }    // Trial Coil
        public string CrewId { get; set; }   // Production Crew ---->  popolare tendina con lookup 'cboCrew' (vedi postman)
        public int ShiftId { get; set; }     // Production Shift ---->  popolare tendina con lookup 'cboShift' (vedi postman)
        public string Remark { get; set; }  // Note
        public int SoakTime { get; set; }  // Soak Time
        public int GapTime { get; set; }   // Gap Time
        public int RollingTime { get; set; }   // Rolling Time
        public int StripCrownIntolFlag { get; set; }  // [TAB Measured Data] Strip Profile [mm] ---> colonna 5 ("In Tolerance")
        public int ExitThkIntolFlag { get; set; }     // [TAB Measured Data] Exit Thickness [mm] ---> colonna 5 ("In Tolerance")
        public int ExitWidthIntolFlag { get; set; }      // [TAB Measured Data] Exit Width [mm] ---> colonna 5 ("In Tolerance")
        public int ExitTempIntolFlag { get; set; }         // [TAB Measured Data] Finishing Temperature [^C] ---> colonna 5 ("In Tolerance")
        public int DowncoilTempIntolFlag { get; set; }     // [TAB Measured Data] Coiling Temperature [^C] ---> colonna 5 ("In Tolerance")
        public int InterstandCoolingBitmask { get; set; }   // Mappatura per campi 
        public int ExitThk { get; set; }  // Exit Thickness
        public int InPieceNo { get; set; }
        public double CalculatedWeight { get; set; }
        public double MeasuredWeight { get; set; }  // Measured Weight
        public double Length { get; set; }  // Exit Length
        public double EntryLength { get; set; }  // Entry Length
        public double InnerDiameter { get; set; }   // Internal Diameter
        public double OuterDiameter { get; set; }    // External Diameter
        public DateTime ProductionStartDate { get; set; }   // Start Production Time
        public DateTime ProductionStopDate { get; set; }    // Stop Production Time
        public int Status { get; set; }  // Coil Status ---->  popolare tendina con lookup 'cboExCoilStatus' (vedi postman)
        public string Operator { get; set; }    // Operator
        public DateTime Revision { get; set; }    // Revision
        public double StripCrownTolPerc { get; set; }   // [TAB Measured Data] Strip Profile [mm] ---> colonna 4 ("In Tol[%]")
        public double ExitThkTolPerc { get; set; }      // [TAB Measured Data] Exit Thickness [mm] ---> colonna 4 ("In Tol[%]")
        public double ExitWidthTolPerc { get; set; }  // [TAB Measured Data] Exit Width [mm] ---> colonna 4 ("In Tol[%]")
        public double ExitTempTolPerc { get; set; }     // [TAB Measured Data] Finishing Temperature [^C] ---> colonna 4 ("In Tol[%]")
        public double DowncoilTempTolPerc { get; set; }   // [TAB Measured Data] Coiling Temperature [^C] ---> colonna 4 ("In Tol[%]")
        public double TotalReduction { get; set; }   // Total Reduction
        public string HeatId { get; set; }  // Heat No
        public string MaterialGradeId { get; set; }    // Material Grade ID
        public string GradeGroupId { get; set; }   // Grade Group ID
        public double TargetColdWidth { get; set; }   // Target Cold Width
        public double ExitWidth { get; set; }   // Exit Width
        public int TargetColdThk { get; set; }
        public int ExitTemp { get; set; }
        public string CustomerId { get; set; }   // Customer ID
        public double EntryHeadWidth { get; set; }     // Entry Head Width
        public double EntryHeadThickness { get; set; }     // Entry Head Thickness
        public int UsedDefaultChemComp { get; set; }   // Default Chemistry Used
        public Boolean[] DispositionCodesForHold { get; set; }  // TAB 'DISPOSITION DATA'



    }

    public class ProducedCoilsDto : ProducedCoilsBaseDto
    {

    }

    public class ProducedCoilsListItemDto : ProducedCoilsDto
    {
        public string StatusLabel { get; set; }
    }

    public class ProducedCoilsForInsertDto : ProducedCoilsBaseDto
    {
        public string OutPieceArea { get; set; }
        public int DestCodeId { get; set; }
        public int CombinationNumber { get; set; }

    }

    public class ProducedCoilsForUpdateDto : ProducedCoilsBaseDto
    {	
    }
    
    public class ProducedCoilsLookupDto
    {
        public string Display { get; set; }
        public int Value { get; set; }
    }

    public class ProducedCoilsDetailDto : ProducedCoilsDto
    {
        public int JobPieceSeq { get; set; }  // Line Piece No
        public string OrderNumber { get; set; }  // Customer Order No
        public string OrderPosition { get; set; }   // Order Line No
        public InputCoilTrgMeasDetailDto InputCoilTrgMeas { get; set; }   // TAB 'MEASURED DATA'
        public OutCoilSetupIntermediateTempDetailDto OutCoilSetupIntermediateTemp { get; set; }  // TAB 'MEASURED DATA'  (Intermediate temperature)
        public RepHmRollDetailDto[] RollDataForStands { get; set; }  // TAB 'ROLLS DATA'
        public CurrentOutCoilMeasDto[] CurrentOutCoilMeas { get; set; }  // TAB 'MEASURED DATA' ("Tabella" di destra")

    }

    public class ProducedCoilsListFilterDto
    {
        public DateTimeOffset? SearchProductionStopDateFrom { get; set; }
        public DateTimeOffset? SearchProductionStopDateTo { get; set; }
        public string SearchProducedPieceId { get; set; }
        public string SearchInputSlabNumber { get; set; }
        public string SearchHeatNumber { get; set; }
        public int? SearchCoilStatus { get; set; }
    }
}