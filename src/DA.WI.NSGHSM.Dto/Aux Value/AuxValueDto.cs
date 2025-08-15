using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DA.WI.NSGHSM.Dto.AuxValue
{

    public class AuxValueBaseDto
    {
        public string ValueLabel { get; set; }
        public int IntegerValue { get; set; }
        public string CharValue { get; set; }
        public string ValueName { get; set; }
    }

    public class AuxValueDto : AuxValueBaseDto
    {
    }

    public class AuxValueListItemDto : AuxValueDto
    {
    }
    [DataContract]
    public class ValidationRangeDTO
    {
        public string valueName { get; set; }
        [DataMember]
        public float max { get; set; }
        [DataMember]
        public float min { get; set; }
        [DataMember]
        public string unitSI { get; set; }
        [DataMember]
        public string unitUSCS { get; set; }
    }

    public class CustomValidationDTO
    {
        public Dictionary<string, ValidationRangeDTO> rangeValidation { get; set; }
    }

    public class AuxValueListFilterDto
    {
        public string SearchVariableId { get; set; }
    }
}