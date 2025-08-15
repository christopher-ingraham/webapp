using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class RepHmSetupAllStepsBaseDto
    {
    }

    public class RepHmSetupAllStepsDto : RepHmSetupAllStepsBaseDto
    {
    }

    public class RepHmSetupAllStepsListItemDto : RepHmSetupAllStepsDto
    {
        public int StepNo { get; set; }
        public int PassNo { get; set; }
        public int StandNo { get; set; }       // Stand No
        public int EnabledStand { get; set; }   // Enabled Stand  ---->  lookup cboEnbledStand  (Postaman ---> lookups)
        public int AgcConMode { get; set; }
        public double EntryWidth { get; set; }      // Entry/Exit Width  (Entry)
        public double ExitWidth { get; set; }       // Entry/Exit Width  (Exit)
        public double EntryThk { get; set; }     // Entry/Exit Thickness  (Entry)
        public double ExitThk { get; set; }      // Entry/Exit Thickness  (Exit)
        public int EntryLength { get; set; }
        public int ExitLength { get; set; }
        public int PvrThkHead { get; set; }
        public int PvrLenHead { get; set; }
        public int PvrThkTail { get; set; }
        public int PvrLenTail { get; set; }
        public int EdgerTrgWidth { get; set; }
        public int EdgerForce { get; set; }
        public int EdgerSpeed { get; set; }
        public int EdgerStiffness { get; set; }
        public int EdgerDeltaWdtHead { get; set; }
        public int EdgerDeltaLenHead { get; set; }
        public int EdgerDeltaWdtTail { get; set; }
        public int EdgerDeltaLenTail { get; set; }
        public int EntryStripRough { get; set; }
        public int ExitStripRough { get; set; }
        public double EntryTemp { get; set; }       // Entry/Exit Temp (Entry)
        public double ExitTemp { get; set; }        // Entry/Exit Temp  (Exit)
        public double SpecEntryTension { get; set; }    // Entry/Exit Tension[kN] (Entry)
        public double SpecExitTension { get; set; }     // Entry/Exit Tension[kN]  (Exit)
        public int LooperAngle { get; set; }
        public double ThreadingSpeed { get; set; }   // Threading Speed
        public double MillSpeed { get; set; }     // Body Max Speed
        public int Fslip { get; set; }
        public int Bslip { get; set; }
        public double Draft { get; set; }       // Draft
        public int BiteAngle { get; set; }
        public int Reduction { get; set; }      // Reduction
        public int Gap { get; set; }
        public int Stiffness { get; set; }
        public double Force { get; set; }    // Body Force
        public int MotorTorque { get; set; }
        public int MotorPower { get; set; }
        public int AvgYs { get; set; }
        public int MuEst { get; set; }
        public int StretchEstEn { get; set; }
        public double HEntryTemp { get; set; }    // Entry/Exit Tension[MPa] (Entry)
        public double HExitTemp { get; set; }     // Entry/Exit Tension[MPa]  (Exit)
        public int HEnrtyWidth { get; set; }
        public int HExitWidth { get; set; }
        public int HEntryThk { get; set; }
        public int HExitThk { get; set; }
        public int HGap { get; set; }
        public int HStiffness { get; set; }
        public double HFslip { get; set; }       // Head Forward Slip
        public int HBslip { get; set; }      
        public double HForce { get; set; }      //  Head Force
        public int HMotorTorque { get; set; }
        public int HMotorPower { get; set; }
        public int HWrBend { get; set; }
        public int HGapOffset { get; set; }
        public int EntryShearGap { get; set; }
        public int ExitShearGap { get; set; }
        public int ExitCrown { get; set; }
        public int ExitCrownElle { get; set; }
        public int PassTargetProfileA2 { get; set; }
        public int PassTargetProfileA4 { get; set; }
        public double WrBend { get; set; }        //  WR Bending
        public int DwrBend { get; set; }
        public int GapOffset { get; set; }
        public int ThExpOffset { get; set; }
        public double WrBendSlope { get; set; }    //   Bending/Force Slope
        public int WrBendCrown { get; set; }
        public int WrShift { get; set; }     //  WR Shifting
        public int HeadNoCooling { get; set; }
        public int TailNoCooling { get; set; }
        public int WrCoolingFlow { get; set; }
        public int WrCoolingNarrow { get; set; }
        public int WrCoolingMiddle { get; set; }
        public int WrCoolingWide { get; set; }
        public int StripCoolingFlow { get; set; }
        public int ForceToEntryThk { get; set; }
        public int ForceToExitThk { get; set; }
        public int ForceToEntryTens { get; set; }
        public int ForceToExitTens { get; set; }
        public int ForceToMillSpeed { get; set; }
        public int ValiditySpeedRange { get; set; }
        public int SpeedToExitTemp { get; set; }
        public int StripCoolToExitTemp { get; set; }
        public int WrbToFlatness { get; set; }
        public int WrbToProfile { get; set; }
        public int RollExtTemp { get; set; }
        public int ThkLastDev { get; set; }
        public double RollBiteOilPerc { get; set; }   // Oil in Water [Ancillary Setup]
        public double HeadCropLength { get; set; }    //  Crop Shear Head Cut length
        public double TailCropLength { get; set; }    //  Crop Shear Tail Cut length
    }

    public class RepHmSetupAllStepsForInsertDto : RepHmSetupAllStepsBaseDto
    {
    }

    public class RepHmSetupAllStepsForUpdateDto : RepHmSetupAllStepsBaseDto
    {
    }

    public class RepHmSetupAllStepsDetailDto : RepHmSetupAllStepsDto
    {


    }

    public class RepHmSetupAllStepsListFilterDto
    {
    }
}