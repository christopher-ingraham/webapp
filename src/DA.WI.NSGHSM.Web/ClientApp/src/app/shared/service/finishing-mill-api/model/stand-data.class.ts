import { Type } from 'class-transformer';

import { StepStatus } from '../../cooling-downcoilers-api/model/step-status.class';
import { StandParams } from './stand-params.class';

export class StandData  {
    @Type(() => StepStatus)
    status: StepStatus;
    @Type(() => StandParams)
    actualData: StandParams;
    @Type(() => StandParams)
    setupData: StandParams;
}
