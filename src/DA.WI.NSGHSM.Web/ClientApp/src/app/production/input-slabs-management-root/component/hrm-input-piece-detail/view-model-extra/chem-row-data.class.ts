import { ChemRowDataColumn } from './chem-row-data-column.class';

export class ChemRowData {
    public left: ChemRowDataColumn;
    public right: ChemRowDataColumn;
    constructor(leftLabel: string, rightLabel: string) {
        this.left = new ChemRowDataColumn(leftLabel);
        this.right = new ChemRowDataColumn(rightLabel);
    }
}
