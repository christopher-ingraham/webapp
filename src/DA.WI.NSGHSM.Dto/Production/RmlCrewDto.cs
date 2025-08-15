using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class RmlCrewBaseDto
    {
    }

    public class RmlCrewDto : RmlCrewBaseDto
    {
    }

    public class RmlCrewListItemDto : RmlCrewDto
    {
    }

    public class RmlCrewForInsertDto : RmlCrewBaseDto
    {
    }

    public class RmlCrewForUpdateDto : RmlCrewBaseDto
    {
    }

    public class RmlCrewDetailDto : RmlCrewDto
    {
    }

    public class RmlCrewLookupDto
    {
        public string Display { get; set; }
        public string Value { get; set; }
    }

     public class RmlCrewListFilterDto 
    {
    }
}