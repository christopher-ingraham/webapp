import { Type } from 'class-transformer';

export class EntityValidationMetadataMap<TValidationMetadataProperty> {
    // @Type(() => TValidationMetadataProperty)
    [property: string]: TValidationMetadataProperty;
}

