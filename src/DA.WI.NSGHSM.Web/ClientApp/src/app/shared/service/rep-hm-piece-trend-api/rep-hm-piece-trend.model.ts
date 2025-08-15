// rep-hm-piece-trend.model.ts
import { Type, Exclude } from 'class-transformer';

import {
    EntityListFilter,
    FilterItem,
    FilterItemNumber,
    FilterItemString,
} from '../../model';
import { RepHmPiece } from '../rep-hm-piece-api';

export class RepHmPieceTrendBase {
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

export class RepHmPieceTrend extends RepHmPieceTrendBase {
    id: number;

    @Exclude()
    public get title(): string {
        const description = this.description || '(no description)';
        return `${description} [${this.measUnit}]`;
    }
}

export class RepHmPieceTrendDetail extends RepHmPieceTrend {
    // nothing
}

export class RepHmPieceTrendForInsert extends RepHmPieceTrend {
    // nothing
}

export class RepHmPieceTrendForUpdate extends RepHmPieceTrend {
    // nothing
}

export class RepHmPieceTrendListFilterFromRoot {
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

export class RepHmPieceTrendListItem extends RepHmPieceTrend {
    // nothing
}

export class RepHmPieceTrendListFilter
    extends EntityListFilter<RepHmPieceTrendListFilter> {

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
