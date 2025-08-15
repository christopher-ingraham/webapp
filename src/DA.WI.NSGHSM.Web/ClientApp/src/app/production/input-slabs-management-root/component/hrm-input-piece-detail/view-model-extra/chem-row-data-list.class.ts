import { ChemRowData } from './chem-row-data.class';
import { ChemRowDataColumn } from './chem-row-data-column.class';
import { ChemicalLabels } from './chemical-labels.interface';

export class ChemRowDataList extends Array<ChemRowData> {

    constructor(chemicalLabels: ChemicalLabels) {
        super();
        this.push(
            new ChemRowData(chemicalLabels.carbon, chemicalLabels.aluminium),
            new ChemRowData(chemicalLabels.manganese, chemicalLabels.nitrogen),
            new ChemRowData(chemicalLabels.phosphorus, chemicalLabels.vanadium),
            new ChemRowData(chemicalLabels.sulphur, chemicalLabels.niobium),
            new ChemRowData(chemicalLabels.silicon, chemicalLabels.titanium),
            new ChemRowData(chemicalLabels.copper, chemicalLabels.calcium),
            new ChemRowData(chemicalLabels.nickel, chemicalLabels.tungsten),
            new ChemRowData(chemicalLabels.chromium, chemicalLabels.boron),
            new ChemRowData(chemicalLabels.molybdenum, chemicalLabels.cobalt),
            new ChemRowData(chemicalLabels.tin, chemicalLabels.lead),
        );
        Object.setPrototypeOf(this, ChemRowDataList.prototype);
    }


    public setAnalysis(label: string, value: number) {
        this.setValue(label, value, 'analysis');
    }
    public setMin(label: string, value: number) {
        this.setValue(label, value, 'min');
    }
    public setMax(label: string, value: number) {
        this.setValue(label, value, 'max');
    }
    public setNo(label: string, value: number) {
        this.setValue(label, value, 'no');
    }

    private findLeft(label: string): ChemRowDataColumn {
        const rowData = this.find((item) => (label === item.left.label));
        if (rowData) {
            return rowData.left;
        }
    }
    private findRight(label: string): ChemRowDataColumn {
        const rowData = this.find((item) => (label === item.right.label));
        if (rowData) {
            return rowData.right;
        }
    }
    private setValue(label: string, value: number, propertyName: string) {
        const item = this.findLeft(label) || this.findRight(label);
        if (item && item.hasOwnProperty(propertyName)) {
            item[propertyName] = value;
        }
    }
}
