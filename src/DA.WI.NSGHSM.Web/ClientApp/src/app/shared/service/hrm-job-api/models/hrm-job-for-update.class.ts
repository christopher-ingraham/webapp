import { HrmJobBase } from './hrm-job-base.class';

export class HrmJobForUpdate extends HrmJobBase {
    public orderEndDate?: string; // Date

    constructor(options?: Partial<HrmJobForUpdate>) {
        super();
        if (options) {
            Object.assign(this, options);
        }
    }

}
