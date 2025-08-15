import { Type } from 'class-transformer';

import { StepStatus } from './step-status.class';
import { IntPyrometerActualParams } from './int-pyrometer-actual-params.class';
import { IntPyrometerSetupParams } from './int-pyrometer-setup-params.class';

export class IntPyrometerData {
    @Type(() => StepStatus)
    status: StepStatus;
    @Type(() => IntPyrometerActualParams)
    actualData: IntPyrometerActualParams;
    @Type(() => IntPyrometerSetupParams)
    setupData: IntPyrometerSetupParams;
}