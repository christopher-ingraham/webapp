import { Validators } from '@angular/forms';
import { Type, Exclude } from 'class-transformer';

import { EntityValidationMetadataMap } from './entity-validation-metadata-map.class';
import { EntityValidationMetadataPropertyRange } from './entity-validation-metadata-property-range.class';
import { ValidatorFn } from '@angular/forms';
import { UomValuePipe } from '../pipe';
import { isString } from 'util';
import { SystemOfUnitsInfo } from './system-of-units-info.class';
import { AppStatusStoreService } from '../service/app-status-store/app-status-store.service';

export class EntityValidationMetadata<TEntityDetail> {
    // internal properties to be masked
    private readonly myName = EntityValidationMetadata['name'];
    private readonly constraintSet: string[] = [];
    public uomValuePipe: UomValuePipe;

    // Constraint set(s)
    @Type(() => EntityValidationMetadataMap)
    rangeValidation: EntityValidationMetadataMap<EntityValidationMetadataPropertyRange> = {};

    constructor() {
        // Mask internal properties
        this.constraintSet = Object.keys(this).filter((key) => !['myName', 'constraintSet'].includes(key));
    }

    @Exclude()
    public getValidators(propertyName: keyof TEntityDetail): ValidatorFn[] {
        const validators: ValidatorFn[] = [];

        this.constraintSet.forEach((validationKind) => {
            switch (validationKind) {
                case 'rangeValidation':
                    const validator = this.getRangeValidators(propertyName);
                    if (validator) {
                        validators.push(...validator);
                    }
                    break;
                default:
                    throw new Error(`unknown validation "${validationKind}"`);
            }
        });
        return validators;
    }

    private getRangeValidators(propertyName: keyof TEntityDetail): ValidatorFn[] {
        const def = this.rangeValidation[propertyName as string] as EntityValidationMetadataPropertyRange;

        if (def) {
            if(!this.uomValuePipe.getAppStatus.isUomSI){
                def.min = this.uomValuePipe.transform(def.min, def.unitSI, def.unitUSCS);
                def.max = this.uomValuePipe.transform(def.max, def.unitSI, def.unitUSCS);
            }

            return [Validators.min(def.min), Validators.max(def.max)];
        }

        return [];
    }
}

