using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Dto._Core
{
    public class PageRequestDto
    {
        public long Skip { get; set; }
        public long Take { get; set; }
    }

    public class ListRequestDto<TFilterDto>
    {
        public TFilterDto Filter { get; set; }
        public PageRequestDto Page { get; set; }
        public SortRequestDto Sort { get; set; }
    }


    public class ListResultDto<TListItemDto>
    {
        public TListItemDto[] Data { get; set; }
        public long Total { get; set; }
    }

    public class SortRequestDto
    {
        public SortRequestItemDto[] Items { get; set; }
    }


    public class SortRequestItemDto
    {
        public string FieldName { get; set; }
        public bool IsDescending { get; set; }
    }

}
