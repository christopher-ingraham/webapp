import { Type, Exclude } from 'class-transformer';

import {
    EntityListFilter,
    FilterItem,
    FilterItemNumber,
    FilterItemString,
} from '../../model';
import { RepHmPiece } from '../rep-hm-piece-api';

export class RepHmMapsViewBase {
    mapNo: number;
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

export class RepHmMapsView extends RepHmMapsViewBase {
    id: number;

    @Exclude()
    public get title(): string {
        const description = this.description || '(no description)';
        return `${description} [${this.measUnit}]`;
    }
}

export class RepHmMapsViewDetail extends RepHmMapsView {
    // nothing
}

export class RepHmMapsViewForInsert extends RepHmMapsView {
    // nothing
}

export class RepHmMapsViewForUpdate extends RepHmMapsView {
    // nothing
}

export class RepHmMapsViewListFilterFromRoot {
    public centerIdEq: FilterItemString;
    public chartData: FilterItemNumber;
    public outPieceNoEq: FilterItemNumber;
    public passNoEq: FilterItemNumber;
    public sampleIdEq: FilterItemString;
    public sampleIdNe: FilterItemString;
    public SignalTypeEq: FilterItemNumber;

    constructor(repHmPiece?: RepHmPiece) {
        if (repHmPiece) {
            this.centerIdEq = new FilterItemString(repHmPiece.outPieceArea, true);
            this.chartData = new FilterItemNumber(0, true);
            this.outPieceNoEq = new FilterItemNumber(repHmPiece.outPieceNo, true);
            this.passNoEq = new FilterItemNumber(repHmPiece.passNo, true);
            this.sampleIdEq = new FilterItemString(null, false);
            this.sampleIdNe = new FilterItemString('SAMPLE_OFFSET', true);
            this.SignalTypeEq = new FilterItemNumber(null, false);
        } else {
            this.centerIdEq = new FilterItemString();
            this.chartData = new FilterItemNumber(0, true);
            this.outPieceNoEq = new FilterItemNumber();
            this.passNoEq = new FilterItemNumber();
            this.sampleIdEq = new FilterItemString(null, false);
            this.sampleIdNe = new FilterItemString('-1', true);
            this.SignalTypeEq = new FilterItemNumber(null, false);
        }
    }
}

export class RepHmMapsViewListItem extends RepHmMapsView {
    // nothing
}

export class RepHmMapsViewListFilter
    extends EntityListFilter<RepHmMapsViewListFilter> {

    centerIdEq: string;
    chartData: string;
    outPieceNoEq: string;
    passNoEq: string;
    sampleIdEq: string;
    sampleIdNe: string;
    signalTypeEq: string;

    constructor() {
        super();
        this.centerIdEq = null;
        this.chartData = '0';
        this.outPieceNoEq = null;
        this.passNoEq = null;
        this.sampleIdEq = null;
        this.sampleIdNe = '-1';
        this.signalTypeEq = null;
    }
}
