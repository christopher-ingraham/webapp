using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class RollDataForStandsBaseDto
    {

    }

    public class RollDataForStandsDto : RollDataForStandsBaseDto
    {
        public RepHmRollDetailDto[] Stands { get; set; }
    }

    public class RollDataForStandsListItemDto : RollDataForStandsDto
    {

    }

    public class RollDataForStandsForInsertDto : RollDataForStandsBaseDto
    {
    }

    public class RollDataForStandsForUpdateDto : RollDataForStandsBaseDto
    {
        
    }

    public class RollDataForStandsDetailDto : RollDataForStandsDto
    {
    }

    public class RollDataForStandsListFilterDto
    {
    }
}