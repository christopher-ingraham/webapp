using System.Collections.Generic;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.AuxValue;


namespace DA.WI.NSGHSM.IRepo.AuxValue
{
    public interface IAuxValueRepo<TDataSource>
    {
        ListResultDto<AuxValueListItemDto> SelAuxIntegerValue(ListRequestDto<AuxValueListFilterDto> listRequest);

        CustomValidationDTO getRangeValidationDTO(string variableId, Dictionary<KeyValuePair<string, string>, KeyValuePair<string, string>> valueList);
    }
}
