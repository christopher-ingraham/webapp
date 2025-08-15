import { Type } from 'class-transformer';


import { NormalData } from './normal-data.class';
import { TrimmingData } from './trimming-data.class';
import { RollssmallData } from './rollssmall-data.class';
import { DowncoilerData } from './downcoiler-data.class';
import { EntryPyrometerData } from './entry-pyrometer-data.class';
import { IntPyrometerData } from './int-pyrometer-data.class';
import { ExitPyrometerData } from './exit-pyrometer-data.class';
import { IntensiveData } from './intensive-data.class';
import { GeneralData } from './general-data.class';

export class CoolingDowncoilersData {
    @Type(() => IntensiveData)
    intensiveData: IntensiveData[];
    @Type(() => NormalData)
    normalData: NormalData[];
    @Type(() => TrimmingData)
    trimmingData: TrimmingData[];
    @Type(() => RollssmallData)
    rollssmallData: RollssmallData;
    @Type(() => DowncoilerData)
    downcoilerData: DowncoilerData[];
    @Type(() => EntryPyrometerData)
    entryPyrometerData: EntryPyrometerData;
    @Type(() => IntPyrometerData)
    intPyrometerData: IntPyrometerData;
    @Type(() => ExitPyrometerData)
    exitPyrometerData: ExitPyrometerData;
    @Type(() => GeneralData)
    generalData: GeneralData;
}
