import { Type } from 'class-transformer';

import { FinishingMillEntryData } from './finishing-mill-entry-data.class';
import { FinishingMillExitData } from './finishing-mill-exit-data.class';
import { StandData } from './stand-data.class';
import { FinishingMillGeneralData } from './finishing-mill-general-data.class';
import { FinishingMillDescalerData } from './finishing-mill-descaler-data.class';

export class FinishingMillData {
    @Type(() => StandData)
    standData: StandData[];
    @Type(() => FinishingMillEntryData)
    entryData: FinishingMillEntryData;
    @Type(() => FinishingMillExitData)
    exitData: FinishingMillExitData;
    @Type(() => FinishingMillGeneralData)
    generalData: FinishingMillGeneralData[];
    @Type(() => FinishingMillDescalerData)
    descalerData: FinishingMillDescalerData;
}
