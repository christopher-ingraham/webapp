import { EntityListFilter } from '../../../model/entity-list-filter.class';

export class HrmInputPieceListFilter
    extends EntityListFilter<HrmInputPieceListFilter> {


    public searchCreationTimeFrom?: string;
    public searchCreationTimeTo?: string;
    public searchCustomerName: string;
    public searchCustomerOrderNo?: string;
    public searchHeatNo?: string;
    public searchPieceStatus?: string;
    public searchProductionStatusFrom?: string;
    public searchProductionStatusTo?: string;
    public searchSlabNo: string;
    public searchStatusLike?: string;
    public searchStringNo?: string;

}
