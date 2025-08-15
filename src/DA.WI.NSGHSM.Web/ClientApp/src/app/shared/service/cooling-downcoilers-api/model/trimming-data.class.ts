import { Type } from 'class-transformer';

import { StepStatus } from './step-status.class';
import { TrimmingParams } from './trimming-params.class';

export class TrimmingData {
    @Type(() => StepStatus)
    topStatus: StepStatus;
    @Type(() => StepStatus)
    bottomStatus: StepStatus;
    @Type(() => TrimmingParams)
    actualData: TrimmingParams;
}
