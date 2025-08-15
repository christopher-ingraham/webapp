import { Type } from 'class-transformer';

import { StepStatus } from '../../cooling-downcoilers-api/model/step-status.class';

export class DescalerData {
    @Type(() => StepStatus)
    status: StepStatus;
}
