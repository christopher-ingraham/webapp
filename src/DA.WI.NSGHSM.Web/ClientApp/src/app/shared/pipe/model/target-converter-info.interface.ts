import { SourceTargetConverterInfo } from './source-target-converter-info.interface';

export interface TargetConverterInfo {
    [target: string]: SourceTargetConverterInfo;
}
