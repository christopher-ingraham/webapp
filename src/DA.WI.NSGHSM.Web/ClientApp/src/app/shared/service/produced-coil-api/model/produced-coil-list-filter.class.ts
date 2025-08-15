// ProducedCoil-list-filter.class.ts
export class ProducedCoilListFilter {
    public searchProductionStopDateFrom?: string;
    public searchProductionStopDateTo?: string;
    public searchProducedPieceId?: string;
    public searchInputSlabNumber?: string;
    public searchHeatNumber?: string;
    public searchCoilStatus?: string;

    constructor() {
        this.searchProductionStopDateFrom = null;
        this.searchProductionStopDateTo = null;
        this.searchProducedPieceId = null;
        this.searchInputSlabNumber = null;
        this.searchHeatNumber = null;
        this.searchCoilStatus = null;
    }
}
