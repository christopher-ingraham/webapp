export class ChemRowDataColumn {
    public analysis: number;
    public max: number;
    public min: number;
    public no?: number;
    constructor(public label: string) {
        this.analysis = 0.0;
        this.max = 0.0;
        this.min = 0.0;
        this.no = 0.0;
    }
}
