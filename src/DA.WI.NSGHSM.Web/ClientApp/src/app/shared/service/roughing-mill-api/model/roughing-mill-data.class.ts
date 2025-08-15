import { Type } from 'class-transformer';

import { DescalerData } from './descaler-data.class';
import { EdgerData } from './edger-data.class';
import { AfterData } from './after-data.class';
import { StandData } from '../../finishing-mill-api/model/stand-data.class';
import { IntensiveData } from '../../cooling-downcoilers-api/model/intensive-data.class';
import { GeneralData } from './general-data.class';


export class RoughingMillData {
    @Type(() => DescalerData)
    descalerData: DescalerData;
    @Type(() => EdgerData)
    edgerData: EdgerData;
    @Type(() => StandData)
    standData: StandData[];
    @Type(() => IntensiveData)
    intensiveData: IntensiveData[];
    @Type(() => AfterData)
    afterData: AfterData;
    @Type(() => GeneralData)
    generalData: GeneralData[];
}
