import { Type } from 'class-transformer';

import { StepStatus } from './step-status.class';

export class RollssmallData {
    @Type(() => StepStatus)
    status: StepStatus;
}
