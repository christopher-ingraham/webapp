import { UnitOfMeasurement, UomValuePipe } from '../pipe';

export class UomValueConverterInfo {
    constructor(
        public readonly uomSI: UnitOfMeasurement,
        public readonly uomUSCS: UnitOfMeasurement,
        private readonly uomValuePipe: UomValuePipe
    ) { }

    public toView(value: number): number {
        return this.uomValuePipe.transform(value, this.uomSI, this.uomUSCS, false);
    }

    public fromView(value: number): number {
        return this.uomValuePipe.transform(value, this.uomUSCS, this.uomSI, false);
    }
}
