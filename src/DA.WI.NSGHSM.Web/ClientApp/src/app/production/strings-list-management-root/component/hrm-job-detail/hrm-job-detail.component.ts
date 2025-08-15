// hrm-job-detail.component.ts
import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';

import { TranslateService } from '@ngx-translate/core';

import {
    AppUserInfoService,
    AuthService,
    LogService,
    ValidationsDecoderService,
} from '@app/core';

import {
    BaseEntityDetailComponent,
    ComboBoxItemNumberString,
    FormGroupEasyBuilder,
    FormGroupUomConverterMap,
    HrmJob,
    HrmJobApiService,
    HrmJobBase,
    HrmJobDetail,
    HrmJobForInsert,
    HrmJobForUpdate,
    HrmJobListFilter,
    HrmJobListItem,
    TabstripTabMetadata,
    UomValuePipe,
    AuxValueService,
} from '@app/shared';

import { ConfirmService } from '@app/widget';

import { StringsListManagementChildState, } from '../../model';

@Component({
    selector: 'app-hrm-job-detail',
    templateUrl: './hrm-job-detail.component.html',
    styleUrls: ['./hrm-job-detail.component.scss']
})
export class HrmJobDetailComponent
    extends BaseEntityDetailComponent<
    HrmJobBase,
    HrmJob,
    HrmJobDetail,
    HrmJobForInsert,
    HrmJobForUpdate,
    HrmJobListItem,
    HrmJobListFilter,
    HrmJobApiService>
    implements OnInit, OnDestroy {

    public childState: StringsListManagementChildState;
    @Input() public parentTab: TabstripTabMetadata<HrmJobListItem>;

    public readonly statusJobList: Observable<ComboBoxItemNumberString[]>;

    constructor(
        formBuilder: FormBuilder,
        appUserInfoService: AppUserInfoService,
        confirmService: ConfirmService,
        log: LogService,
        validationsDecoder: ValidationsDecoderService,
        authService: AuthService,
        private auxValueService: AuxValueService,
        private translateService: TranslateService,
        private hrmJobApiService: HrmJobApiService,
        uomValuePipe: UomValuePipe,
    ) {
        super(formBuilder, log, authService, appUserInfoService,
            confirmService, validationsDecoder, 'EXAMPLE', 'HrmJob', uomValuePipe);
        this.statusJobList = this.createStatusList();
    }

    get service() {
        return this.hrmJobApiService;
    }

    get formGroup(): FormGroup {
        this.disabledControlNames.push('jobSeq', 'totalNumberOf');
        const builder = new FormGroupEasyBuilder(this.formValidationMetadata, this.isReadOnly);
        builder.addDate('orderStartDate', [Validators.required]);
        builder.addDate('revision');
        builder.addNumber('status', [Validators.required]);
        builder.addNumbers('jobSeq', 'totalNumberOf');
        builder.addString('jobId', [Validators.required]);
        builder.addStrings('operator', 'remark', 'statusLabel');
        return builder.formGroup;
    }

    get formGroupConverters(): FormGroupUomConverterMap {
        const cm = new FormGroupUomConverterMap(this.form, this.uomValuePipe);
        // TODO declare any required conversion
        return cm;
    }

    get formToEntityForInsert(): HrmJobForInsert {
        const rawValues: (keyof HrmJobForInsert)[] = [
            'jobId',
            'jobSeq',
            'orderEndDate',
            'orderStartDate',
            'remark',
            'status',
        ];
        const options = this.getFormValuesForInsert(rawValues, []);
        return new HrmJobForInsert(options);
    }

    get formToEntityForUpdate(): HrmJobForUpdate {
        const rawValues: (keyof HrmJobForUpdate)[] = [
            'jobId',
            'jobSeq',
            'orderStartDate',
            'orderEndDate',
            'remark',
            'status',
        ];
        const options = this.getFormValuesForUpdate(rawValues, []);
        return new HrmJobForUpdate(options);
    }

    get newEntityDetail(): Observable<Partial<HrmJob>> {
        return new Observable<Partial<HrmJob>>((obs) => {
            const now = new Date;
            const yyyy = now.getFullYear();
            const mm = 1 + now.getMonth();
            const dd = now.getDate();
            const jobId = `${yyyy}${(mm > 9) ? mm : '0' + mm}${(dd > 9) ? dd : '0' + dd}00`;
            const aa = this.childState;

            this.hrmJobApiService.nextHrmJobSequence.subscribe((jobSeq) => {
                // TODO check what are initial attributes for a new HrmJob
                const newEntity: Partial<HrmJob> = {
                    jobId,
                    jobSeq,
                    status: 10,
                };
                obs.next(newEntity);
            });
        });
    }

    ngOnInit(): void {
        this.detailInit().subscribe((ok) => {
            if (ok) {
                /* anything after form+valid+convertes initialization */
            }
        });
    }

    onSaveClick() {
        // FIXME: is this guard actually required?
        // [Save] is disabled, if !form.valid
        // [Save] is not present, if isReadonly
        if (!this.form.valid || this.isReadOnly === true) {
            return;
        }
        this.clearServerErrorMap();
        if (this.isNew) {
            // create new HrmJob
            const entityForInsert = this.formToEntityForInsert;
            this.sanitizeId();
            if (this.id) {
                entityForInsert.jobId = this.id.toString();
            }
            // TODO fill entityForInsert (HrmJobForInsert) own fields
            this.hrmJobApiService.create(entityForInsert).subscribe(
                (result) => {
                    this.goBack();
                },
                (error) => {
                    this.processServerErrorMap(error);
                }
            );
        } else {
            // update existing HrmJob
            const entityForUpdate = this.formToEntityForUpdate;
            this.sanitizeId();
            if (this.id) {
                entityForUpdate.jobId = this.id.toString();
            }
            // TODO fill entityForUpdate (HrmJobForUpdate) own fields
            this.hrmJobApiService.update(this.id, entityForUpdate).subscribe(
                (result) => {
                    this.goBack();
                },
                (error) => {
                    this.processServerErrorMap(error);
                }
            );
        }
    }

    public onDeleteClick() {
        const title = 'Delete String';
        const question = this.translateService.instant(this.areYouSureToDeleteThisItem);
        this.clearServerErrorMap();
        this.deleteConfirm(title, question).subscribe((deleted) => {
            if (deleted) {
                this.refreshData(this.id);
            }
        });
    }

    private sanitizeId() {
        // if id = NaN backend side the controller crashes
        if (isNaN(this.id)) {
            this.id = undefined;
        }
    }

    private createStatusList(): Observable<ComboBoxItemNumberString[]> {
        return this.auxValueService.statusJobList;
    }
}
