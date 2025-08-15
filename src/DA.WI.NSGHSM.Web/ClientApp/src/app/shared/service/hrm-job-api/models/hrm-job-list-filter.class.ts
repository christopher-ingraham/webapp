import { EntityListFilter } from '../../../model';

export class HrmJobListFilter
    extends EntityListFilter<HrmJobListFilter> {

    public searchDataFrom: string;
    public searchDataTo: string;
    public searchJobId: string;
    public searchProductionStatus: string;

}
