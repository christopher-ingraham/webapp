import { Type } from 'class-transformer';

import { StepStatus } from '../../cooling-downcoilers-api';
import { AfterSetupParams } from './after-setup-params.class';
import { AfterActualParams } from './after-actual-params.class';

export class AfterData {
    @Type(() => StepStatus)
    statusDto: StepStatus;
    @Type(() => AfterSetupParams)
    setupData: AfterSetupParams;
    @Type(() => AfterActualParams)
    actualData: AfterActualParams;
}

