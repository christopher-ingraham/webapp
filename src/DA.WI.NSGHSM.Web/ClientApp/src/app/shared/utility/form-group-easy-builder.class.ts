import { FormGroup, AbstractControl, FormControl, ValidatorFn } from '@angular/forms';

import { EntityValidationMetadata } from '../model';

export class FormGroupEasyBuilder<TEntityDetail> {
    private controls: { [key: string]: AbstractControl } = {};

    constructor(
        private readonly validationMetadata: EntityValidationMetadata<TEntityDetail>,
        private readonly disabled: boolean = false,
    ) {
        if (!validationMetadata) {
            throw new Error(`${FormGroupEasyBuilder['name']}: validationMetadata required but found "${validationMetadata}"`);
        }
    }

    public get formGroup(): FormGroup {
        return new FormGroup(this.controls);
    }

    public addBoolean(
        name: string,
        customValidators: ValidatorFn[] = [],
        value = { value: false, disabled: this.disabled },
    ) {
        this.addFormControl(name, value, customValidators);
    }
    public addBooleans(...names: string[]) {
        names.forEach((name) => this.addBoolean(name));
    }

    public addDate(
        name: string,
        customValidators: ValidatorFn[] = [],
        value = { value: (new Date()), disabled: this.disabled },
    ) {
        this.addFormControl(name, value, customValidators);
    }
    public addDates(...names: string[]) {
        names.forEach((name) => this.addDate(name));
    }

    public addNumber(
        name: string,
        customValidators: ValidatorFn[] = [],
        value = { value: 0, disabled: this.disabled },
    ) {
        this.addFormControl(name, value, customValidators);
    }
    public addNumbers(...names: string[]) {
        names.forEach((name) => this.addNumber(name));
    }

    public addString(
        name: string,
        customValidators: ValidatorFn[] = [],
        value = { value: '', disabled: this.disabled },
    ) {
        this.addFormControl(name, value, customValidators);
    }
    public addStrings(...names: string[]) {
        names.forEach((name) => this.addString(name));
    }

    public addFormControl(name: string, value: any, customValidators: ValidatorFn[]) {
        const backendValidators = this.validationMetadata.getValidators(name as (keyof TEntityDetail));
        this.controls[name] = new FormControl(value, [...backendValidators, ...customValidators]);
    }
}
