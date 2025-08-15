import { Type } from 'class-transformer';

import { StepStatus } from '../../cooling-downcoilers-api/model/step-status.class';
import { FinishingMillSetupEntryParams } from './finishing-mill-setup-entry-params.class';
import { FinishingMillActualEntryParams } from './finishing-mill-actual-entry-params.class';


export class FinishingMillEntryData {
    @Type(() => StepStatus)
    status: StepStatus;
    @Type(() => FinishingMillSetupEntryParams)
    setupData: FinishingMillSetupEntryParams;
    @Type(() => FinishingMillActualEntryParams)
    actualData: FinishingMillActualEntryParams;
}
