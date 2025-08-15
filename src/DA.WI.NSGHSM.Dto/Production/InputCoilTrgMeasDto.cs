using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class InputCoilTrgMeasBaseDto
    {

    }

    public class InputCoilTrgMeasDto : InputCoilTrgMeasBaseDto
    {

    }

    public class InputCoilTrgMeasListItemDto : InputCoilTrgMeasDto
    {

    }

    public class InputCoilTrgMeasForInsertDto : InputCoilTrgMeasBaseDto
    {
    }

    public class InputCoilTrgMeasForUpdateDto : InputCoilTrgMeasBaseDto
    {
    }

    public class InputCoilTrgMeasDetailDto : InputCoilTrgMeasDto
    {
        public double TargetWidth { get; set; }    // Exit Width [Target]
        public double TargetWidthPtol { get; set; }   // Exit Width [Min]
        public double TargetWidthNtol { get; set; }    // Exit Width [Max]
        public double TargetThickness { get; set; }    // Exit Thickness [Target]
        public double TargetThicknessPtol { get; set; }  // Exit Thickness [Min]
        public double TargetThicknessNtol { get; set; }   // Exit Thickness [Max]
        public double TargetTempFm { get; set; }         // Finishing Temperature [Target]
        public double TargetTempFmPtol { get; set; }     // Finishing Temperature [Min]
        public double TargetTempFmNtol { get; set; }      // Finishing Temperature [Max]
        public double TargetTempDc { get; set; }         // Coiling Temperature [Target]
        public double TargetTempDcPtol { get; set; }     // Coiling Temperature [Min]
        public double TargetTempDcNtol { get; set; }     // Coiling Temperature [Max]
        public double TargetProfile { get; set; }       // Strip Profile [Target]
        public double TargetProfilePtol { get; set; }     // Strip Profile [Min]
        public double TargetProfileNtol { get; set; }     // Strip Profile [Max]

    }

    public class InputCoilTrgMeasListFilterDto
    {
    }
}