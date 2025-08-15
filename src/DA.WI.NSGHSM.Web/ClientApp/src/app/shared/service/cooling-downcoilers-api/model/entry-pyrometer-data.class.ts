import { Type } from 'class-transformer';

import { StepStatus } from './step-status.class';
import { EntryPyrometerActualParams } from './entry-pyrometer-actual-params.class';
import { EntryPyrometerSetupParams } from './entry-pyrometer-setup-params.class';

export class EntryPyrometerData {
    @Type(() => StepStatus)
    status: StepStatus;
    @Type(() => EntryPyrometerActualParams)
    actualData: EntryPyrometerActualParams;
    @Type(() => EntryPyrometerSetupParams)
    setupData: EntryPyrometerSetupParams;
}
