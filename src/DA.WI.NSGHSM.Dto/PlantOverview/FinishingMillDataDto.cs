
namespace DA.WI.NSGHSM.Dto.PlantOverview
{

    public class FinishingMillDataDto
    {
        public int PieceNo  { get; set; }
        public FinishingMillStandDataDto[] StandData { get; set; } // 0-5
        public FinishingMillEntryData EntryData { get; set; }
        public FinishingMillExitData ExitData { get; set; }
        public FinishingMillGeneralData[] GeneralData { get; set; }
        public FinishingMillDescalerDataDto DescalerData { get; set; } 

    }

    public class FinishingMillDescalerDataDto
    {
        public StepStatusDto Status { get; set; }
    }

    public class FinishingMillEntryData
    {
        public FinishingMillEntrySetupValueDto SetupData { get; set; }
        public FinishingMillEntryActualValueDto ActualData { get; set; }

    }

    public class FinishingMillExitData
    {
        public FinishingMillExitSetupValueDto SetupData { get; set; }
        public FinishingMillExitActualValueDto ActualData { get; set; }
    }

    public class FinishingMillGeneralData
    {
        public int PositionNo { get; set; }
        public string PositionLabel { get; set; }
        public string PieceId { get; set; }
        public int PieceNo { get; set; }
        public string FurnaceTime { get; set; }
    }

    public class FinishingMillStandDataDto
    {
        public StepStatusDto Status { get; set; }
        public FinishingMillStandValueDto SetupData { get; set; }
        public FinishingMillStandValueDto ActualData { get; set; }
    }

    public class FinishingMillEntryActualValueDto
    {
        public int StepNo { get; set; }
        public double EntryWidth { get; set; }
        public double EntrySpeed { get; set; }
        public int EntrySpeedStatus { get; set; }
        public int EntryTempStatus { get; set; }
        public double EntryTemperature1 { get; set; }
        public double EntryTemperature2 { get; set; }
        public double EntryTemperature3 { get; set; }
    }


        public class FinishingMillEntrySetupValueDto
    {
        public int StepNo { get; set; }
        public double EntryWidth { get; set; }
        public double EntrySpeed { get; set; }
        public double EntryTemp { get; set; }
    }

    public class FinishingMillExitSetupValueDto
    {
        public double StepNo { get; set; }
        public double ExitThickness { get; set; }
        public double ExitWidth { get; set; }
        public double ExitSpeed { get; set; }
        public double Flatness { get; set; }
        public double Crown { get; set; }
         public int CrownDistance { get; set; }
        public double ExitTemp { get; set; }

    }

        public class FinishingMillExitActualValueDto
    {
        public double StepNo { get; set; }
        public double ExitThickness { get; set; }
        public int ExitThicknessStatus { get; set; }
        public double ExitWidth { get; set; }
        public int ExitWidthStatus { get; set; }
        public double ExitSpeed { get; set; }
        public int ExitSpeedStatus { get; set; }
        public double Flatness { get; set; }
        public int FlatnessStatus { get; set; }
        public double StripCrown25 { get; set; }
        public double StripCrown40 { get; set; }
        public int CrownStatus { get; set; }
        public double StripWedge25 { get; set; }
        public double StripWedge40 { get; set; }
        public int WedgeStatus { get; set; }
        public int ExitTempStatus { get; set; }
        public double ExitTemp1 { get; set; }
        public double ExitTemp2 { get; set; }
        public double ExitTemp3 { get; set; }

    }


    public class FinishingMillOtherValueDto
    {
        public int StepNo { get; set; }
        public double ExitThickness { get; set; }
        public double ExitWidth { get; set; }
        public double ExitSpeed { get; set; }
        public double Flatness { get; set; }
        public double CrownC25 { get; set; }
        public double CrownC40 { get; set; }

    }

    public class FinishingMillStandValueDto
    {
        public int StepNo { get; set; }
        public double ExitThickness { get; set; }
        public double RollingForce { get; set; }
        public double StandSpeed { get; set; }
        public double Reduction { get; set; }
        public double Gap { get; set; }
        public double WRBending { get; set; }
        public double WRShifting { get; set; }
        public double LooperAngle { get; set; }
        public double LooperSpecificTension { get; set; }
        public double InterstandCoolingFlow { get; set; }
    }
}