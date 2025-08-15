using System;
namespace DA.WI.NSGHSM.Dto.PlantOverview
{
    public class CoolingDowncoilersDataDto
    {
        public int PieceNo  { get; set; }
        public DowncoilersDataDto[] DowncoilerData { get; set; } // 0-1
        public EntryPyrometerDataDto EntryPyrometerData { get; set; }
        public IntPyrometerDataDto IntPyrometerData { get; set; }
        public ExitPyrometerDataDto ExitPyrometerData { get; set; }
        public IntensiveDataDto[] IntensiveData { get; set; }
        public NormalDataDto[] NormalData { get; set; }
        public TrimmingDataDto[] TrimmingData { get; set; }
        public RollssmallData RollssmallData { get; set; }
        public CoolingDowncoilerGeneralData[] GeneralData { get; set; }

    }

    public class IntensiveDataDto
    {
       
        public StepStatusDto TopStatus { get; set; }
      
        public StepStatusDto BottomStatus { get; set; }
     
    }

    public class NormalDataDto
    {
        public StepStatusDto TopStatus { get; set; }
        public StepStatusDto BottomStatus { get; set; }
        public NormalDataValueDto ActualData { get; set; }
    }

    public class TrimmingDataDto
    {
        public StepStatusDto TopStatus { get; set; }
        public StepStatusDto BottomStatus { get; set; }
        public TrimmingDataValueDto ActualData { get; set; }
    }

    public class RollssmallData
    {
        public StepStatusDto Status { get; set; }
    }

    public class DowncoilersDataDto
    {
        public StepStatusDto Status { get; set; }
        public Boolean ActiveDC { get; set; }
        public DowncoilersDataSetupValueDto SetupData { get; set; }
        public DowncoilersDataActualValueDto ActualData { get; set; }
    }

    public class EntryPyrometerDataDto
    {
        public StepStatusDto Status { get; set; }
        public EntryPyrometerSetupValueDto SetupData { get; set; }
        public EntryPyrometerActualValueDto ActualData { get; set; }
    }

    public class IntPyrometerDataDto
    {
        public StepStatusDto Status { get; set; }
        public IntPyrometerSetupValueDto SetupData { get; set; }
        public IntPyrometerActualValueDto ActualData { get; set; }
    }

    public class ExitPyrometerDataDto
    {
        public StepStatusDto Status { get; set; }
        public ExitPyrometerSetupValueDto SetupData { get; set; }
        public ExitPyrometerActualValueDto ActualData { get; set; }
    }

    public class ExitPyrometerActualValueDto
    {
        public double ExitTemp1LR { get; set; }
        public int ExitTemp1LRStatus { get; set; }
        public double ExitTemp2LR { get; set; }
        public int ExitTemp2LRStatus { get; set; }
        public double ExitTemp3LR { get; set; }
        public int ExitTemp3LRStatus { get; set; }
        public double ExitTemp1HR { get; set; }
        public int ExitTemp1HRStatus { get; set; }
        public double ExitTemp2HR { get; set; }
        public int ExitTemp2HRStatus { get; set; }
        public double ExitTemp3HR { get; set; }
        public int ExitTemp3HRStatus { get; set; }
    }

      public class ExitPyrometerSetupValueDto
    {
        public double TargetTemp { get; set; }

    }

    public class EntryPyrometerActualValueDto
    {
        public double EntryTemp1 { get; set; }
        public int EntryTemp1Status { get; set; }
        public double EntryTemp2 { get; set; }
        public int EntryTemp2Status { get; set; }
        public double EntryTemp3 { get; set; }
        public int EntryTemp3Status { get; set; }

    }

    public class EntryPyrometerSetupValueDto
    {
        public double TargetTemp { get; set; }

    }

    public class IntPyrometerActualValueDto
    {
        public double IntTemp1 { get; set; }
        public int IntTemp1Status { get; set; }
        public double IntTemp2 { get; set; }
        public int IntTemp2Status { get; set; }
        public double IntTemp3 { get; set; }
        public int IntTemp3Status { get; set; }

    }

        public class IntPyrometerSetupValueDto
    {
        public double TargetTemp { get; set; }

    }

    public class DowncoilersDataActualValueDto
    {
        public int StepNo { get; set; }
        public string PieceId { get; set; }
        public double InternalDiameter { get; set; }
        public double ExternalDiameter { get; set; }
        public double SpecificTensions { get; set; }
        public double StripSpeed { get; set; }
    }

    
    public class DowncoilersDataSetupValueDto
    {
        public int StepNo { get; set; }
        public double TrgInnerDiameter { get; set; }
        public double ExitTension { get; set; }
        public double MillSpeed { get; set; }
    }

    public class IntensiveDataValueDto
    {
        public int StepNo { get; set; }
    }

    public class NormalDataValueDto
    {
        public int StepNo { get; set; }
    }

    public class TrimmingDataValueDto
    {
        public int StepNo { get; set; }
    }

     public class CoolingDowncoilerGeneralData
    {
        public int PositionNo { get; set; }
        public string PositionLabel { get; set; }
        public string PieceId { get; set; }
        public int PieceNo { get; set; }
        public string FurnaceTime { get; set; }
    }

}
