
// rep-hm-piece-Trend-detail.component.ts
import { ActivatedRoute, Router, Route } from '@angular/router';
import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl, FormBuilder } from '@angular/forms';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';

import { TranslateService } from '@ngx-translate/core';

import { AuthService, ValidationsDecoderService, AppUserInfoService, LogService } from '@app/core';
import {
    BaseEntityDetailComponent,
    ChildState,
    RepHmPieceTrend,
    RepHmPieceTrendApiService,
    RepHmPieceTrendBase,
    RepHmPieceTrendDetail,
    RepHmPieceTrendForInsert,
    RepHmPieceTrendForUpdate,
    RepHmPieceTrendListFilter,
    RepHmPieceTrendListItem,
    FormGroupUomConverterMap,
    UomValuePipe,
    TabstripTabMetadata,
} from '@app/shared';
import { ConfirmService } from '@app/widget';
import { classToClass } from 'class-transformer';


@Component({
    selector: 'app-rep-hm-piece-trend-detail',
    templateUrl: './rep-hm-piece-trend-detail.component.html',
    styleUrls: ['./rep-hm-piece-trend-detail.component.scss']
})
export class RepHmPieceTrendDetailComponent
    extends BaseEntityDetailComponent<
    RepHmPieceTrendBase,
    RepHmPieceTrend,
    RepHmPieceTrendDetail,
    RepHmPieceTrendForInsert,
    RepHmPieceTrendForUpdate,
    RepHmPieceTrendListItem,
    RepHmPieceTrendListFilter,
    RepHmPieceTrendApiService>
    implements OnInit, OnDestroy {

    public childState: ChildState;

    @Input() public parentTab: TabstripTabMetadata<RepHmPieceTrendListItem>;

    constructor(
        formBuilder: FormBuilder,
        appUserInfoService: AppUserInfoService,
        confirmService: ConfirmService,
        log: LogService,
        validationsDecoder: ValidationsDecoderService,
        authService: AuthService,
        private repHmPieceTrendApiService: RepHmPieceTrendApiService,
        private translateService: TranslateService,
        uomValuePipe: UomValuePipe,
    ) {
        super(formBuilder, log, authService, appUserInfoService,
            confirmService, validationsDecoder, 'EXAMPLE', 'RepHmPieceTrend', uomValuePipe);
    }

    get service() {
        return this.repHmPieceTrendApiService;
    }

    get formGroup(): FormGroup {
        return new FormGroup({
            description: new FormControl(this.formControlDefaultStringState, []),
            measUnit: new FormControl('', []),
            centerId: new FormControl('', []),
            outPieceNo: new FormControl('', []),
            passNo: new FormControl('', []),
            signalId: new FormControl('', []),
        });
    }
    get formGroupConverters(): FormGroupUomConverterMap {
        const cm = new FormGroupUomConverterMap(this.form, this.uomValuePipe);
        // TODO
        return cm;
    }


    get formToEntityForInsert(): RepHmPieceTrendForInsert {
        return classToClass<RepHmPieceTrendForInsert>(this.form.value);
    }
    get formToEntityForUpdate(): RepHmPieceTrendForUpdate {
        return classToClass<RepHmPieceTrendForUpdate>(this.form.value);
    }
    get newEntityDetail() {
        return this.newEntityDetailConstant({});
    }

    ngOnInit(): void {
        this.detailInit().subscribe((ok) => {
            if (ok) {
                /* anything after form+valid+convertes initialization */
            }
        });
    }

    onSaveClick() {
        if (!this.form.valid || this.isReadOnly === true) {
            return;
        }
        const data = this.isNew ? this.formToEntityForInsert : this.formToEntityForUpdate;
        // if id = NaN backend side the controller crashes
        if (isNaN(this.id)) {
            this.id = undefined;
        }
        data.id = this.id;
        const apiAction = this.isNew
            ? this.repHmPieceTrendApiService.create(data as RepHmPieceTrendForInsert)
            : this.repHmPieceTrendApiService.update(this.id, data as RepHmPieceTrendForUpdate);

        apiAction.subscribe(
            (result) => this.goBack()
        );
    }

    onDeleteClick() {
        if (!this.id || this.isReadOnly === true) {
            return;
        }

        if (!confirm(this.translateService.instant(this.areYouSureToDeleteThisItem))) {
            return;
        }

        this.repHmPieceTrendApiService.delete(this.id).subscribe(
            (result) => this.goBack()
        );
    }

}

