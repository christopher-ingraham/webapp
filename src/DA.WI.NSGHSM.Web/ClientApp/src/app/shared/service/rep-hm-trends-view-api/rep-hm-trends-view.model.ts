// rep-hm-piece-trend.model.ts
import { Type, Exclude } from 'class-transformer';

import {
    EntityListFilter,
    FilterItem,
    FilterItemNumber,
    FilterItemString,
} from '../../model';
import { RepHmPiece } from '../rep-hm-piece-api';

export class RepHmTrendsViewBase {
    signalNo: number;
    signalId: string;
    description: string;
    measUnit: string;
    outPieceNo: number;
    centerId: number;
    passNo: number;
    sampleData: string;
    chartDataX: number[];
    chartDataY: number[];
    chartDataZ: number[][];
    signalType: number;
    chartType: string;
}

export class RepHmTrendsView extends RepHmTrendsViewBase {
    id: number;

    @Exclude()
    public get title(): string {
        const description = this.description || '(no description)';
        return `${description} [${this.measUnit}]`;
    }
}

export class RepHmTrendsViewDetail extends RepHmTrendsView {
    // nothing
}

export class RepHmTrendsViewForInsert extends RepHmTrendsView {
    // nothing
}

export class RepHmTrendsViewForUpdate extends RepHmTrendsView {
    // nothing
}

export class RepHmTrendsViewListFilterFromRoot {
    public outPieceNoEq: FilterItemNumber;

    constructor(repHmPiece?: RepHmPiece) {
        this.outPieceNoEq = new FilterItemNumber(repHmPiece.outPieceNo, true);
    }
}

export class RepHmTrendsViewListItem extends RepHmTrendsView {
    // nothing
}

export class RepHmTrendsViewListFilter
    extends EntityListFilter<RepHmTrendsViewListFilter> {

    outPieceNoEq: string;

    constructor() {
        super();
        this.outPieceNoEq = null;
    }
}
