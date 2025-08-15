import { UnitOfMeasurement } from '../pipe/model/unit-of-measurement.type';

export interface FormGroupKeyUomMapping {
    key: string;
    uomSI: UnitOfMeasurement;
    uomUSCS: UnitOfMeasurement;
}
