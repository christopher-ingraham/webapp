using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class ExitSaddlesBaseDto
    {
        public int PositionNo { get; set; }
        public string SaddleLabel { get; set; }
        public int PieceNo { get; set; }
        public string JobId { get; set; }
        public string OutPieceId { get; set; }
    }

    public class ExitSaddlesDto : ExitSaddlesBaseDto
    {
    }

    public class ExitSaddlesListItemDto : ExitSaddlesDto
    {
    }

    public class ExitSaddlesForInsertDto : ExitSaddlesBaseDto
    {
    }

    public class ExitSaddlesForUpdateDto : ExitSaddlesBaseDto
    {
    }

    public class ExitSaddlesDetailDto : ExitSaddlesDto
    {
    }

    public class ExitSaddlesListFilterDto
    {

    }
}