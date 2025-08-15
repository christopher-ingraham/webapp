import { Type } from 'class-transformer';

import { StepStatus } from './step-status.class';
import { DowncoilerSetupParams } from './downcoiler-setup-params.class';
import { DowncoilerActualParams } from './downcoiler-actual-params.class';

export class DowncoilerData {
    @Type(() => StepStatus)
    status: StepStatus;
    activeDC: boolean;
    @Type(() => DowncoilerActualParams)
    actualData: DowncoilerActualParams;
    @Type(() => DowncoilerSetupParams)
    setupData: DowncoilerSetupParams;
}
