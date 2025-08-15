using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class OutCoilSetupIntermediateTempBaseDto
    {

    }

    public class OutCoilSetupIntermediateTempDto : OutCoilSetupIntermediateTempBaseDto
    {

    }

    public class OutCoilSetupIntermediateTempListItemDto : OutCoilSetupIntermediateTempDto
    {

    }

    public class OutCoilSetupIntermediateTempForInsertDto : OutCoilSetupIntermediateTempBaseDto
    {
    }

    public class OutCoilSetupIntermediateTempForUpdateDto : OutCoilSetupIntermediateTempBaseDto
    {
    }

    public class OutCoilSetupIntermediateTempDetailDto : OutCoilSetupIntermediateTempDto
    {
        public double TargetTempInterm { get; set; }      // Intermediate temperature {1}
        public double TargetTempIntermUpTol { get; set; }   // Intermediate temperature {3}
        public double TargetTempIntermLoTol { get; set; }   // Intermediate temperature {2}
    }

    public class OutCoilSetupIntermediateTempListFilterDto
    {
    }
}