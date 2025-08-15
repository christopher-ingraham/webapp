
namespace DA.WI.NSGHSM.Dto.PlantOverview
{

    public class RoughingMillDescalerDataDto
    {
        public StepStatusDto Status { get; set; }
    }

    public class RoughingMillAfterDataDto
    {
        public RoughingMillAfterSetupValueDto SetupData { get; set; }
        public RoughingMillAfterActualValueDto ActualData { get; set; }

    }

    public class RoughingMillStandDataDto
    {
        public StepStatusDto Status { get; set; }
        public RoughingMillValueDto SetupData { get; set; }
        public RoughingMillValueDto ActualData { get; set; }
    }

    public class RoughingMillEdgerDataDto
    {
        public StepStatusDto Status { get; set; }
        public RoughingMillEdgerSetupValueDto SetupData { get; set; }
        public RoughingMillEdgerActualValueDto ActualData { get; set; }
    }

    public class RoughingMillIntensiveDataDto
    {
        public StepStatusDto TopStatus { get; set; }
        public StepStatusDto BottomStatus { get; set; }
    }

    public class RoughingMillDataDto
    {
        public int PieceNo { get; set; }
        public RoughingMillDescalerDataDto DescalerData { get; set; }
        public RoughingMillEdgerDataDto EdgerData { get; set; }
        public RoughingMillGeneralData[] GeneralData { get; set; }
        public RoughingMillStandDataDto[] StandData { get; set; } // 0-1
        public RoughingMillIntensiveDataDto[] IntensiveData { get; set; }
        public RoughingMillAfterDataDto AfterData { get; set; }
    }


    public class RoughingMillValueDto
    {
        public int StepNo { get; set; }
        public double ExitThickness { get; set; }
        public double RollingForce { get; set; }
        public double StandSpeed { get; set; }
        public double Reduction { get; set; }
        public double Gap { get; set; }
        public double WRBending { get; set; }
    }
        public class RoughingMillGeneralData
    {
        public int PositionNo { get; set; }
        public string PositionLabel { get; set; }
        public string PieceId { get; set; }
        public int PieceNo { get; set; }
        public string FurnaceTime { get; set; }
    }

    public class RoughingMillAfterSetupValueDto
    {
        public int StepNo { get; set; }
        public double ExitWidth { get; set; }
        public double ExitSpeed { get; set; }
        public double ExitTemp { get; set; }
    }

    public class RoughingMillAfterActualValueDto
    {
        public int StepNo { get; set; }
        public double ExitWidth { get; set; }
        public int ExitWidthStatus { get; set; }
        public double ExitSpeed { get; set; }
        public int ExitSpeedStatus { get; set; }
        public int ExitTempStatus { get; set; }
        public double ExitTemp1 { get; set; }
        public double ExitTemp2 { get; set; }
        public double ExitTemp3 { get; set; }
    }

    public class RoughingMillEdgerSetupValueDto
    {
        public int StepNo { get; set; }
        public double EntryWidth { get; set; }
        public double EntrySpeed { get; set; }
        public double EntryTemp { get; set; }
        public double EdgerWidth { get; set; }
        public double EdgerForce { get; set; }
    }

        public class RoughingMillEdgerActualValueDto
    {
        public int StepNo { get; set; }
        public double EntryWidth { get; set; }
        public int EntryWidthStatus { get; set; }
        public double EntrySpeed { get; set; }
        public int EntrySpeedStatus { get; set; }
        public int EntryTempStatus { get; set; }
        public double EntryTemp1 { get; set; }
        public double EntryTemp2 { get; set; }
        public double EntryTemp3 { get; set; }
        public double EdgerWidth { get; set; }
        public double EdgerForce { get; set; }
    }
}
