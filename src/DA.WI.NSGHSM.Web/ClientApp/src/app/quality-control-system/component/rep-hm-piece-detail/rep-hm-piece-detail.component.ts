
// rep-hm-piece-detail.component.ts
import { ActivatedRoute, Router, Route } from '@angular/router';
import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl, FormBuilder } from '@angular/forms';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';

import { classToClass } from 'class-transformer';

import { TranslateService } from '@ngx-translate/core';

import { AuthService, ValidationsDecoderService, AppUserInfoService, LogService } from '@app/core';
import {
    BaseEntityDetailComponent,
    ChildState,
    RepHmPiece,
    RepHmPieceApiService,
    RepHmPieceBase,
    RepHmPieceDetail,
    RepHmPieceForInsert,
    RepHmPieceForUpdate,
    RepHmPieceListFilter,
    RepHmPieceListItem,
    FormGroupUomConverterMap,
    UomValuePipe,
    TabstripTabMetadata,
} from '@app/shared';
import { ConfirmService } from '@app/widget';


@Component({
    selector: 'app-rep-hm-piece-detail',
    templateUrl: './rep-hm-piece-detail.component.html',
    styleUrls: ['./rep-hm-piece-detail.component.scss']
})
export class RepHmPieceDetailComponent
    extends BaseEntityDetailComponent<
    RepHmPieceBase,
    RepHmPiece,
    RepHmPieceDetail,
    RepHmPieceForInsert,
    RepHmPieceForUpdate,
    RepHmPieceListItem,
    RepHmPieceListFilter,
    RepHmPieceApiService>
    implements OnInit, OnDestroy {

    public childState: ChildState;
    @Input() public parentTab: TabstripTabMetadata<RepHmPieceListItem>;

    constructor(
        formBuilder: FormBuilder,
        appUserInfoService: AppUserInfoService,
        confirmService: ConfirmService,
        log: LogService,
        validationsDecoder: ValidationsDecoderService,
        authService: AuthService,
        private repHmPieceApiService: RepHmPieceApiService,
        private translateService: TranslateService,
        uomValuePipe: UomValuePipe,
    ) {
        super(formBuilder, log, authService, appUserInfoService,
            confirmService, validationsDecoder, 'EXAMPLE', 'RepHmPiece', uomValuePipe);
    }

    get service() {
        return this.repHmPieceApiService;
    }

    get formGroup(): FormGroup {
        return new FormGroup({
            data: new FormControl(this.formControlDefaultDateState, []),
            materialGradeId: new FormControl(null, []), // TODO
            outputCoil: new FormControl(null, []), // TODO
            thickness: new FormControl(this.formControlDefaultNumberState, []),
            weight: new FormControl(this.formControlDefaultNumberState, []),
            width: new FormControl(this.formControlDefaultNumberState, []),
        });
    }
    get formGroupConverters(): FormGroupUomConverterMap {
        const cm = new FormGroupUomConverterMap(this.form, this.uomValuePipe);
        cm.map = [
            { key: 'width', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'thickness', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'weight', uomSI: 'kg', uomUSCS: 'lb' },
        ];
        return cm;
    }


    get formToEntityForInsert(): RepHmPieceForInsert {
        return classToClass<RepHmPieceForInsert>(this.form.value);
    }
    get formToEntityForUpdate(): RepHmPieceForUpdate {
        return classToClass<RepHmPieceForUpdate>(this.form.value);
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
            ? this.repHmPieceApiService.create(data)
            : this.repHmPieceApiService.update(this.id, data);

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

        this.repHmPieceApiService.delete(this.id).subscribe(
            (result) => this.goBack()
        );
    }

}
