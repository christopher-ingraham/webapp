
using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class UsedSetupBaseDto
    {
    }

    public class UsedSetupDto : UsedSetupBaseDto
    {
    }

    public class UsedSetupListItemDto : UsedSetupDto
    {
        public int InPieceNo { get; set; }
        public int StepNo { get; set; }
        public int PassNo { get; set; }
        public int StandNo { get; set; }
        public string StandNoLabel { get; set; }
        public int EnabledStand { get; set; }
        public string EnabledStandLabel { get; set; }
        public double EntryThk { get; set; }
        public double ExitThk { get; set; }
        public double Draft { get; set; }
        public double Reduction { get; set; }
        public double EntryWidth { get; set; }
        public double ExitWidth { get; set; }
        public double ExitTemp { get; set; }
        public double EntryTension { get; set; }
        public double ExitTension { get; set; }
        public double SpecEntryTens { get; set; }
        public double SpecExitTens { get; set; }
        public double ThreadingSpeed { get; set; }
        public double HForce { get; set; }
        public double MillSpeed { get; set; }
        public double Force { get; set; }
        public double WrBend { get; set; }
        public double WrShift { get; set; }
        public double StripCoolingFlow { get; set; }

    }

    public class UsedSetupForInsertDto : UsedSetupBaseDto
    {
    }

    public class UsedSetupForUpdateDto : UsedSetupBaseDto
    {
    }

    public class UsedSetupDetailDto : UsedSetupDto
    {

    }
    

    public class UsedSetupListFilterDto
    {
        public int? SearchInPieceNo { get; set; }
    }
}
