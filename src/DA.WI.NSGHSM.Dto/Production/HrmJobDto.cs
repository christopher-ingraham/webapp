using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class HrmJobBaseDto
    {
        public string JobId { get; set; }
        public int JobSeq { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public DateTime OrderStartDate { get; set; }
        public string Operator { get; set; }
        public DateTime Revision { get; set; }

    }

    public class HrmJobDto : HrmJobBaseDto
    {
        public string StatusLabel { get; set; }
    }

    public class HrmJobListItemDto : HrmJobDto
    {
    }

    public class HrmJobForInsertDto : HrmJobBaseDto
    {
        public DateTime? OrderEndDate { get; set; }
    }

    public class HrmJobForUpdateDto : HrmJobBaseDto
    {
        public DateTime? OrderEndDate { get; set; }
    }

    public class HrmJobDetailDto : HrmJobDto
    {
        public int TotalNumberOf { get; set; }
    }

     public class HrmJobListFilterDto
    {
        public string SearchJobId { get; set; }
        public int? SearchProductionStatus { get; set; }
        public DateTimeOffset? SearchDataFrom { get; set; }
        public DateTimeOffset? SearchDataTo { get; set; }
    }

    public class HrmJobLookupDto
    {
        public string Display { get; set; }
        public string Value { get; set; }
    }


}
