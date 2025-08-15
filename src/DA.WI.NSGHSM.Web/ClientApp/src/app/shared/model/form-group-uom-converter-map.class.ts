import { FormGroup } from '@angular/forms';

import { UnitOfMeasurement, UomValuePipe } from '../pipe';

import { UomValueConverterInfo } from './uom-value-converter-info.class';
import { FormGroupKeyUomMapping } from './form-group-key-uom-mapping.interface';

export class FormGroupUomConverterMap {

    private controlUomConverterList: Map<string, UomValueConverterInfo>;

    /**
     * @param form form group to attach UoM conversion informations to
     * @param uomValuePipe Ng pipe that will perform the conversion
     */
    constructor(
        private form: FormGroup,
        private uomValuePipe: UomValuePipe
    ) {
        this.controlUomConverterList = new Map<string, UomValueConverterInfo>();
    }

    /**
     * @description attach UoM conversion information to a control
     * @param key target control in form
     * @param uomSI DTO unit of measurement (SI)
     * @param uomUSCS VIEW unit of measurement (USCS)
     */
    public add(
        key: string,
        uomSI: UnitOfMeasurement,
        uomUSCS: UnitOfMeasurement
    ) {
        const vci = new UomValueConverterInfo(uomSI, uomUSCS, this.uomValuePipe);
        this.controlUomConverterList.set(key, vci);
    }

    /**
     * @description quick way to define a map;
     * @see add
     */
    public set map(value: FormGroupKeyUomMapping[]) {
        (value || []).forEach(
            ({ key, uomSI, uomUSCS }) => this.add(key, uomSI, uomUSCS)
        );
    }
    public get map(): FormGroupKeyUomMapping[] {
        return Array.from(this.controlUomConverterList, ([key, { uomSI, uomUSCS }]) => ({ key, uomSI, uomUSCS }));
    }

    /**
     * @abstract patch a form, possibly converting input values
     * @param value see FormGroup.patchValue
     */
    public patchValue(value: { [key: string]: any }): void {
        Object.keys(value).forEach((key) => {
            const newValue = value[key];
            const conversionInfo = this.controlUomConverterList.get(key);
            if (conversionInfo && (typeof newValue === 'number')) {
                const newValueAsNumber = newValue as number;
                const convertedNewValueObject = { [key]: conversionInfo.toView(newValueAsNumber) };
                this.form.patchValue(convertedNewValueObject);
            } else {
                // control with NO converter, or non-numeric value
                this.form.patchValue({ [key]: newValue });
            }
        });
    }

    /**
     * @abstract get a value from a form's control, possibly converting
     * output value
     * @param key control's name in form
     */
    public getValue(key: string): number {
        const control = this.form.get(key);
        if (control) {
            const valueFromView = control.value;
            const conversionInfo = this.controlUomConverterList.get(key);
            if (conversionInfo) {
                return conversionInfo.fromView(valueFromView);
            } else {
                // control with NO converter
                return valueFromView;
            }
        }
    }
}
