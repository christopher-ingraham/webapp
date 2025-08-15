import { Type } from 'class-transformer';

import { StepStatus } from '../../cooling-downcoilers-api/model/step-status.class';
import { FinishingMillSetupExitParams } from './finishing-mill-setup-exit-params.class';
import { FinishingMillActualExitParams } from './finishing-mill-actual-exit-params.class';

export class FinishingMillExitData {
    @Type(() => StepStatus)
    status: StepStatus;
    @Type(() => FinishingMillSetupExitParams)
    setupData: FinishingMillSetupExitParams;
    @Type(() => FinishingMillActualExitParams)
    actualData: FinishingMillActualExitParams;
}
