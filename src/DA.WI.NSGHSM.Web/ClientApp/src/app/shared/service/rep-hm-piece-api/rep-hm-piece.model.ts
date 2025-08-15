// rep-hm-piece.model.ts
import { Type } from 'class-transformer';

import {
    EntityListFilter,
    FilterItemFromToDate,
    FilterItemString,
} from '../../model';

export class RepHmPieceBase {
    outPieceNo: number;
    producedCoilId: string;
    inputCoilId: string;
    heatNo: number;
    customerOrderNo: string;
    customerLineNo: string;
    @Type(() => Date)
    producionStopDate: Date;
    steelGradeId: string;
    width: number;
    thickness: number;
    weight: number;
    externalDiameter: number;
    outPieceArea: string;
    passNo: number;
}

export class RepHmPiece extends RepHmPieceBase {
    id: number;
}

export class RepHmPieceDetail extends RepHmPiece {
    // nothing
}

export class RepHmPieceForInsert extends RepHmPiece {
    // nothing
}

export class RepHmPieceForUpdate extends RepHmPiece {
    // nothing
}

// from QCS root component to RepHmPieceList component
export class RepHmPieceListFilterFromRoot {
    public producedCoilId: FilterItemString;
    public inputCoilId: FilterItemString;
    public productionStopDate: FilterItemFromToDate;
    constructor() {
        this.producedCoilId = new FilterItemString();
        this.inputCoilId = new FilterItemString();
        this.productionStopDate = new FilterItemFromToDate();
    }
}

export class RepHmPieceListItem extends RepHmPiece {
    // nothing
}

// sent to server
export class RepHmPieceListFilter
    extends EntityListFilter<RepHmPieceListFilter> {

    searchProducedCoil: string;
    searchInputCoil: string;
    searchDataFrom: string;
    searchDataTo: string;

    constructor(options?: Partial<RepHmPieceListFilter>) {
        super();
        if (options) {
            Object.assign(this, options);
        } else {
            this.searchProducedCoil = null;
            this.searchInputCoil = null;
            this.searchDataFrom = null;
            this.searchDataTo = null;
        }
    }
}
