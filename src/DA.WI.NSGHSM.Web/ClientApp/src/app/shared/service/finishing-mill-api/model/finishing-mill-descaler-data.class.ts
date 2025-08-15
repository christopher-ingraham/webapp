
import { Type } from 'class-transformer';
import { StepStatus } from '../../cooling-downcoilers-api/model/step-status.class';

export class FinishingMillDescalerData  {
    @Type(() => StepStatus)
    status: StepStatus;
}
