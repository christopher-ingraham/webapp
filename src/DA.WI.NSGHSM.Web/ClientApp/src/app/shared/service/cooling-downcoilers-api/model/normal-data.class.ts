import { Type } from 'class-transformer';

import { StepStatus } from './step-status.class';
import { NormalParams } from './normal-params.class';

export class NormalData {
    @Type(() => StepStatus)
    topStatus: StepStatus;
    @Type(() => StepStatus)
    bottomStatus: StepStatus;
    @Type(() => NormalParams)
    actualData: NormalParams;
}
