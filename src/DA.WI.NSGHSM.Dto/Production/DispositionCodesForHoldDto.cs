using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class DispositionCodesForHoldBaseDto
    {

    }

    public class DispositionCodesForHoldDto : DispositionCodesForHoldBaseDto
    {
        public int ReasonCnt { get; set; }
        public int OperCancelFlag { get; set; }
        public int ValueSeq { get; set; }
        public string ValueName { get; set; }
    }

    public class DispositionCodesForHoldListItemDto : DispositionCodesForHoldDto
    {

    }

    public class DispositionCodesForHoldForInsertDto : DispositionCodesForHoldBaseDto
    {
    }

    public class DispositionCodesForHoldForUpdateDto : DispositionCodesForHoldBaseDto
    {
    }

    public class DispositionCodesForHoldDetailDto : DispositionCodesForHoldDto
    {
        public string Description {get; set; }

    }

    public class DispositionCodesForHoldListFilterDto
    {
       public int? SearchOutPieceNo { get; set; }
    }
}