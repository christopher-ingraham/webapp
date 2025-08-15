import { Type } from 'class-transformer';

import { StepStatus } from './step-status.class';

export class IntensiveData {
    @Type(() => StepStatus)
    topStatus: StepStatus;
    @Type(() => StepStatus)
    bottomStatus: StepStatus;
}
