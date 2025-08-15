import { BadRequestType } from './bad-request-type.enum';

export interface BadRequest {
    data: any;
    errorType: BadRequestType;
    key: string;
}
