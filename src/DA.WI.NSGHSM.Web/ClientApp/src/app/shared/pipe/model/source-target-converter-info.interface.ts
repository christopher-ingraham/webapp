import { DecimalPipeParameters } from './decimal-pipe-parameters.interface';

export type UomValueConverter = (value: number | boolean) => number | boolean;

export interface SourceTargetConverterInfo {
    converter: UomValueConverter;
    format?: DecimalPipeParameters;
}
