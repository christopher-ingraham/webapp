import { Type } from 'class-transformer';

import { StepStatus } from '../../cooling-downcoilers-api';
import { EdgerActualParams } from './edger-actual-params.class';
import { EdgerSetupParams } from './edger-setup-params.class';

export class EdgerData {
    @Type(() => StepStatus)
    status: StepStatus;
    @Type(() => EdgerActualParams)
    actualData: EdgerActualParams;
    @Type(() => EdgerSetupParams)
    setupData: EdgerSetupParams;
}
