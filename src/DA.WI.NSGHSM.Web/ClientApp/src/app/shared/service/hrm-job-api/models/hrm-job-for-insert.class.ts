import { HrmJobBase } from './hrm-job-base.class';

export class HrmJobForInsert extends HrmJobBase {
    public orderEndDate?: string; // Date

    constructor(options?: Partial<HrmJobForInsert>) {
        super();
        if (options) {
            Object.assign(this, options);
        }
    }

}
