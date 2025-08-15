import { Type } from 'class-transformer';

export class HrmInputPieceBase {
    jobId: string;
    jobPieceSeq: number;
    length: number;
    materialGradeId: string;
    operator: string;
    pieceId: string;
    pieceNo: number;
    @Type(() => Date)
    revision: Date;
    status: number;
    targetThickness: number;
    targetWidth: number;
    targetInternalDiameter: number;
    thickness: number;
    thicknessHead: number;
    thicknessTail: number;
    weight: number;
    width: number;
    widthHead: number;
    widthTail: number;
    heatId: string;
    customerName: string;
}
