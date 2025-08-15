import { Type } from 'class-transformer';

export class HrmJobBase {
    jobId: string;
    jobSeq: number;
    remark: string;
    status: number;
    @Type(() => Date)
    orderStartDate: Date;
    operator: string;
    @Type(() => Date)
    revision: Date;
}
