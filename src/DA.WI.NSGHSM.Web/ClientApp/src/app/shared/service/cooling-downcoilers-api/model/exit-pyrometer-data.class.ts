import { Type } from 'class-transformer';
import { StepStatus } from './step-status.class';
import { ExitPyrometerActualParams } from './exit-pyrometer-actual-params.class';
import { ExitPyrometerSetupParams } from './exit-pyrometer-setup-params.class';

export class ExitPyrometerData {
    @Type(() => StepStatus)
    status: StepStatus;
    @Type(() => ExitPyrometerActualParams)
    actualData: ExitPyrometerActualParams;
    @Type(() => ExitPyrometerSetupParams)
    setupData: ExitPyrometerSetupParams;
}