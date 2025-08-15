import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { OidcSecurityService } from 'angular-auth-oidc-client';

import {
    ApiService,
    ConfigurationService,
    JsonHelperService,
    LogService,
    NotifierService,
    UrlHelperService,
    ListResult,
    ListRequest,
} from '@app/core';

import { AuxValueItemDTO, AuxValueFilterDTO } from './dto';
import { ComboBoxItemNumberString, ComboBoxItemStringString } from './model';

const baseEntityUrl = 'auxvalue/AuxValue';
const baseEntityListUrl = `${baseEntityUrl}/list`;

const listRequestDefaultPage = { skip: 0, take: 9999 };
const listRequestDefaultSort = { items: [] };

@Injectable({
    providedIn: 'root'
})
export class AuxValueService
    extends ApiService {

    constructor(
        http: HttpClient,
        config: ConfigurationService,
        urlHelperService: UrlHelperService,
        notifier: NotifierService,
        oidcService: OidcSecurityService,
        log: LogService,
        jsonHelperService: JsonHelperService
    ) {
        super(http, config, urlHelperService, notifier, oidcService,
            log, jsonHelperService);
    }

    private newListRequest(searchVariableId: string): ListRequest<AuxValueFilterDTO> {
        return {
            filter: {
                searchVariableId
            },
            page: listRequestDefaultPage,
            sort: listRequestDefaultSort,
        };
    }

    private logError(callerName: string, context: string, variableId: string, error: any): void {
        const errorLines = JSON.stringify(error, null, 4).split('\n');
        errorLines.forEach((errorLine) => {
            this.log.error(`AuxValueService.${callerName}(${context}), ${variableId}) ERROR | ${errorLine}`);
        });
    }

    // ---  READ a variable's list of number values  -------------------

    private dtoListToModelNumberList(dtoList: AuxValueItemDTO[]): ComboBoxItemNumberString[] {
        return dtoList.map((dto) => new ComboBoxItemNumberString(dto.integerValue, dto.valueLabel));
    }
    private readAuxValueNumberList(callerName: string, variableId: string): Observable<ComboBoxItemNumberString[]> {
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            const listRequest = this.newListRequest(variableId);
            this.get<ListResult<AuxValueItemDTO>>(baseEntityListUrl, listRequest).subscribe(
                (result) => {
                    if (result.total) {
                        obs.next(this.dtoListToModelNumberList(result.data));
                    } else {
                        obs.next([]);
                    }
                },
                (error) => {
                    this.logError('readAuxValueNumberList', callerName, variableId, error);
                    obs.next([]);
                },
            );
        });
    }

    // ---  READ a variable's list of string values  -------------------

    private dtoListToModelStringList(dtoList: AuxValueItemDTO[]): ComboBoxItemStringString[] {
        return dtoList.map((dto) => new ComboBoxItemStringString(dto.charValue, dto.valueLabel));
    }
    private readAuxValueStringList(callerName: string, variableId: string): Observable<ComboBoxItemStringString[]> {
        return new Observable<ComboBoxItemStringString[]>((obs) => {
            const listRequest = this.newListRequest(variableId);
            this.get<ListResult<AuxValueItemDTO>>(baseEntityListUrl, listRequest).subscribe(
                (result) => {
                    if (result.total) {
                        obs.next(this.dtoListToModelStringList(result.data));
                    } else {
                        obs.next([]);
                    }
                },
                (error) => {
                    this.logError('readAuxValueStringList', callerName, variableId, error);
                    obs.next([]);
                },
            );
        });
    }

    // ---  Public getters  --------------------------------------------

    // Job status dictionary
    public get statusJobList(): Observable<ComboBoxItemNumberString[]> {
        return this.readAuxValueNumberList('statusJobList', 'STATUS_JOB');
    }

    // Input piece status dictionary
    public get statusInputPieceList(): Observable<ComboBoxItemNumberString[]> {
        return this.readAuxValueNumberList('statusInputPieceList', 'STATUS_INPUT');
    }

    // Input piece transition type dictionary
    public get transitionList(): Observable<ComboBoxItemNumberString[]> {
        return this.readAuxValueNumberList('transitionList', 'TRANSITION');
    }

    // Produced coil status dictionay
    public get producedCoilStateList(): Observable<ComboBoxItemNumberString[]> {
        return this.readAuxValueNumberList('producedCoilStateList', 'STATUS_PRODUCED');
    }

    public get areaTypeList(): Observable<ComboBoxItemStringString[]> {
        return this.readAuxValueStringList('areaTypeList', 'AREA_TYPE');
    }

    // Produced Coils Management - Exit Saddles list refresh timeout (in s)
    public get producedCoilManagementExistSaddleListRefreshTimeout(): Observable<number> {
        return new Observable<number>((obs) => {
            const timeout: number = 15; // s - seconds
            // TODO AuxValueService.producedCoilManagementExistSaddleListRefreshTimeout
            obs.next(timeout);
        });
    }

    // Coil status dictionary
    public get coilStatusList(): Observable<ComboBoxItemNumberString[]> {
        return this.readAuxValueNumberList('coilStatusList', 'STATUS_INPUT');
    }

    // Shift Id dictionary
    public get shiftIdList(): Observable<ComboBoxItemNumberString[]> {
        return this.readAuxValueNumberList('shiftIdList', 'PRODUCTION_SHIFT');
    }

    // Inner diameter dictionary
    public get innerDiameterList(): Observable<ComboBoxItemNumberString[]> {
        return this.readAuxValueNumberList('InnerDiameterList', 'EX_INT_DIAMETER');
    }

    // Material Grade ID dictionary
    public get materialGradeList(): Observable<ComboBoxItemNumberString[]> {
        // TODO AuxValueService.materialGradeList
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            const list: ComboBoxItemNumberString[] = [
                new ComboBoxItemNumberString(1, '1: materialGradeList TODO 1'),
                new ComboBoxItemNumberString(2, '2: materialGradeList TODO 2'),
                new ComboBoxItemNumberString(3, '3: materialGradeList TODO 3'),
            ];
            obs.next(list);
        });
    }
}
