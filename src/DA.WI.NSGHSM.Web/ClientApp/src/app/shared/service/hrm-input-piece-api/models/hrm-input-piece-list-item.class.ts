import { HrmInputPiece } from './hrm-input-piece.class';
import { Type } from '@angular/core';

export class HrmInputPieceListItem extends HrmInputPiece {
    customerName: string;
    gradeGroupId: string;
    gradeGroupLabel: string;
    heatId: string;
    orderNumber: string;
    orderPosition: string;
    statusLabel: string;
    transition: number;
    transitionLabel: string;
    trialFlag: number;
    trialFlagLabel: string;
}
