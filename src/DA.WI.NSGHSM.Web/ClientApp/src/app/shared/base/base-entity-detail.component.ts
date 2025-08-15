import { OnDestroy, isDevMode } from '@angular/core';
import {
    AbstractControl,
    FormBuilder,
    FormGroup,
} from '@angular/forms';
import {
    BehaviorSubject,
    Observable,
} from 'rxjs';

import {
    AppUserInfo,
    AppUserInfoService,
    LogService,
    ValidationsDecoderService,
    AuthService,
} from '@app/core';
import { ConfirmService, NumberFormatOptions, TabStripComponent } from '@app/widget';

import { SubscriptionList, CasedObject } from '../utility';
import { BaseEntityService } from './base-entity.service';
import { BaseEntityAudit } from './base-entity-audit.class';
import {
    BadRequestException,
    BadRequestMap,
    ChildState,
    EntityValidationMetadata,
    FormGroupUomConverterMap,
    TabstripTabMetadata,
} from '../model';
import { UomValuePipe } from '../pipe/uom-value.pipe';
import { AppStatusStoreService } from '../service/app-status-store/app-status-store.service';

enum InitialisationKind {
    existingEntity,
    existingEntityCopy,
    newEntity
}

export abstract class BaseEntityDetailComponent<
    TEntityBase,
    TEntity,
    TEntityDetail,
    TEntityForInsert,
    TEntityForUpdate,
    TEntityListItem,
    TEntityListFilter,
    TApiService extends BaseEntityService<
        TEntityBase,
        TEntity,
        TEntityDetail,
        TEntityForInsert,
        TEntityForUpdate,
        TEntityListItem,
        TEntityListFilter>>
    extends SubscriptionList implements OnDestroy {

    public intFormatOptions: NumberFormatOptions = {
        maximumFractionDigits: 0,
    };

    protected userInfo: AppUserInfo;

    public abstract childState: ChildState;

    // Metadata about the actual parent <kendo-tabstrip-tab>.
    public abstract parentTab: TabstripTabMetadata<any>;

    public form: FormGroup;
    public formWithConverters: FormGroupUomConverterMap;
    public formValidationMetadata: EntityValidationMetadata<TEntityDetail>;

    public get isNew(): boolean {
        if (this.parentTab) {
            return this.parentTab.isNew;
        }
        return false;
    }
    public set isNew(value: boolean) {
        this.formEnabled = value || !this._isReadOnly;
    }
    public get isEdit(): boolean { return !this.isNew; }

    // Role flags: All, Restricted, ReadOnly
    public isInAllRole = false;
    public isInRestrictedRole = false;
    private _isReadOnly = true;
    public get isReadOnly(): boolean { return this._isReadOnly; }
    public set isReadOnly(value: boolean) {
        this._isReadOnly = value;
        this.formEnabled = !value || this.isNew;
    }

    public readonly idSubject = new BehaviorSubject<number>(undefined);
    public get id(): number {
        return this.idSubject.getValue();
    }
    public set id(value: number) {
        this.idSubject.next(value);
    }

    protected readonly i18nPrefix: string;

    public get title(): string {
        let value = `${this.i18nPrefix}.PAGETITLE.`;
        if (this.isNew) {
            value += `NEW|New ${this.entityName}`;
        } else {
            value += `DETAIL|${this.entityName} Default`;
        }
        return value;
    }
    public get cancelBackButton(): string {
        return `${this.i18nPrefix}.${this.isReadOnly ? 'BACK|Back' : 'CANCEL|Cancel'}`;
    }
    protected get areYouSureToDeleteThisItem(): string {
        return `${this.i18nPrefix}.DELETE.CONFIRMEX| Are you sure to delete this item?`;
    }

    protected disabledControlNames = ['operator', 'revision'];

    private badRequestMap?: BadRequestMap;

    constructor(
        protected formBuilder: FormBuilder,
        protected log: LogService,
        private authService: AuthService,
        private appUserInfoService: AppUserInfoService,
        private confirmService: ConfirmService,
        private validationsDecoder: ValidationsDecoderService,
        i18nPrefix = 'EXAMPLE',
        private readonly entityName = 'Entity',
        protected uomValuePipe: UomValuePipe
    ) {
        super();
        this.i18nPrefix = `${i18nPrefix}.${entityName.toUpperCase()}`;
        this.subscribe(
            this.appUserInfoService.get().subscribe(
                (appUserInfo) => {
                    this.userInfo = appUserInfo;
                }
            ),
        );
    }

    abstract get service(): TApiService;
    abstract get formGroup(): FormGroup;
    abstract get formGroupConverters(): FormGroupUomConverterMap;
    abstract get formToEntityForInsert(): TEntityForInsert;
    abstract get formToEntityForUpdate(): TEntityForUpdate;
    abstract get newEntityDetail(): Observable<Partial<TEntityDetail>>;

    protected newEntityDetailConstant(value: Partial<TEntityDetail>): Observable<Partial<TEntityDetail>> {
        return new Observable<Partial<TEntityDetail>>((obs) => obs.next(value));
    }


    private setupSecurityRoleFlags() {
        const authService = this.authService;
        this.subscribe(
            authService.isInAllRole.subscribe((isInRole) => this.isInAllRole = isInRole),
            authService.isInRestrictedRole.subscribe((isInRole) => this.isInRestrictedRole = isInRole),
            authService.isInRestrictedRole.subscribe((isInRole) => this.isReadOnly = isInRole),
        );
    }

    /**
     * @abstract to be called in onNgInit()
     */
    protected detailInit(): Observable<boolean> {
        const component = this;
        const authService = this.authService;
        return new Observable<boolean>((obs) => {
            const postReadOnlyRoleFound = (isInReadonlyRole: boolean) => {
                component.isReadOnly = isInReadonlyRole;
                component.service.validationMetadata.subscribe(
                    (validationMetadata) => {
                        validationMetadata.uomValuePipe = this.uomValuePipe;
                        component.processValidationMetadata(validationMetadata);
                        // Now we can build the form
                        component.form = component.formGroup;
                        component.formWithConverters = component.formGroupConverters;
                        component.whichDetailInit().subscribe(
                            (initKind) => {
                                switch (initKind) {
                                    case InitialisationKind.existingEntity:
                                        component.initFromExistingEntity().subscribe((ok) => obs.next(ok));
                                        break;
                                    case InitialisationKind.existingEntityCopy:
                                        component.initFromExistingEntityCopy().subscribe((ok) => obs.next(ok));
                                        break;
                                    case InitialisationKind.newEntity:
                                        component.initFromNewEntity().subscribe((ok) => obs.next(ok));
                                        break;
                                }
                            }
                        );
                    }
                );
            };
            this.subscribe(
                authService.isInAllRole.subscribe((isInRole) => this.isInAllRole = isInRole),
                authService.isInRestrictedRole.subscribe((isInRole) => this.isInRestrictedRole = isInRole),
                authService.isInReadOnlyRole.subscribe(postReadOnlyRoleFound),
            );
        });
    }
    private whichDetailInit(): Observable<InitialisationKind> {
        return new Observable<InitialisationKind>((obs) => {
            if (this.parentTab.isNew) {
                // NEW ENTITY or NEW-COPY-FROM ENTITY
                if (this.parentTab.copyFromId) {
                    obs.next(InitialisationKind.existingEntityCopy);
                } else {
                    obs.next(InitialisationKind.newEntity);
                }
            } else {
                // EXISTING ENTITY
                if (this.parentTab.id) {
                    obs.next(InitialisationKind.existingEntity);
                } else {
                    this.goBackBecauseWeGotGarbage('this.parentTab.id', this.parentTab.id);
                }
            }
        });
    }
    private processValidationMetadata(validationMetadata: EntityValidationMetadata<TEntityDetail>) {
        // So far, simply copy param
        this.formValidationMetadata = validationMetadata;
    }

    private initFromExistingEntity(): Observable<boolean> {
        // Simply fetch data from backend and set them to the form
        this.id = this.parentTab.id;
        return this.refreshData(this.id);
    }
    private initFromExistingEntityCopy(): Observable<boolean> {
        return new Observable<boolean>((obs) => {
            const id = '' + this.parentTab.copyFromId;
            this.service.read(id).subscribe(
                (entityDetail) => {
                    this.isReadOnly = false;
                    this.isNew = true;
                    this.newEntityDetail.subscribe((newEntity) => {
                        Object.assign(entityDetail, this.applyAudit(entityDetail));
                        this.dataToForm(entityDetail);
                        obs.next(true);
                    });
                },
                (err) => obs.error(err)
            );
        });
    }
    private initFromNewEntity(): Observable<boolean> {
        return new Observable<boolean>((obs) => {
            this.newEntityDetail.subscribe(
                (newEntity) => {
                    this.dataToForm(this.applyAudit(newEntity) as TEntityDetail);
                    obs.next(true);
                });
        });
    }

    private getFormControl(fieldName: string) {
        if (this.form && this.form.controls) {
            return this.form.controls[fieldName];
        }
    }

    protected set formEnabled(value: boolean) {
        if (this.form && this.form.controls) {
            for (const fieldName in this.form.controls) { // 'field' is a string
                if (fieldName) {
                    if (-1 === this.disabledControlNames.indexOf(fieldName)) {
                        const control = this.form.get(fieldName); // 'control' is a FormControl
                        if (value) {
                            control.enable();
                        } else {
                            control.disable();
                        }
                    }
                }
            }
        }
    }

    public isInError(fieldName: string): boolean {
        if (fieldName) {
            const ctrl = this.getFormControl(fieldName);
            if (ctrl) {
                return this.isFieldInClientError(ctrl) || this.isFieldInServerError(fieldName);
            }
        }
        return false;
    }
    protected isFieldInClientError(ctrl: AbstractControl) {
        return (ctrl.invalid && ctrl.touched);
        // && (ctrl.dirty || ctrl.touched));
    }
    protected isFieldInServerError(fieldName: string): boolean {
        if (this.badRequestMap) {
            return this.badRequestMap[fieldName] ? true : false;
        }
        return false;
    }
    public getErrors(fieldName: string): string {
        const errorList: string[] = [];

        if (fieldName) {
            const ctrl = this.getFormControl(fieldName);

            if (this.isFieldInClientError(ctrl)) {
                const msg = this.validationsDecoder.decode(this.i18nPrefix, fieldName, ctrl.errors);
                errorList.push(msg);
            }

            if (this.isFieldInServerError(fieldName)) {
                errorList.push(this.badRequestMap[fieldName].key);
            }
        }

        return errorList.join('\n');
    }




    protected get formControlDefaultStringState() {
        return { value: '', disabled: this.isReadOnly, };
    }
    protected get formControlDefaultNumberState() {
        return { value: 0, disabled: this.isReadOnly, };
    }
    protected get formControlDefaultDateState() {
        return { value: (new Date()), disabled: this.isReadOnly, };
    }
    protected get formControlDefaultBooleanState() {
        return { value: false, disabled: this.isReadOnly, };
    }

    protected goBack(reason?: string): void {
        if (reason) {
            // tslint:disable-next-line
            this.log.debug(reason);
        }
        this.parentTab.close();
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }

    onCancelClick() {
        this.goBack();
    }

    protected openNotImplementedDialog(cls: Function, method: string, details?: string) {
        const notImplemented = `${cls['name']}.${method}(...)\n\nNOT implemented`;
        const message = details ? `${notImplemented}\n\n${details}` : notImplemented;
        alert(message);
    }

    protected deleteConfirm(title: string, question: string): Observable<boolean> {
        return new Observable<boolean>((obs) => {
            this.confirmService.openYesNo(title, question).subscribe(
                (result) => {
                    if (result.isAction && (result.action.text === this.confirmService.ActionTextYes)) {
                        this.service.delete(this.id).subscribe(
                            (deleteResult) => {
                                obs.next(true);
                                this.goBack();
                            }
                        );
                    } else {
                        obs.next(false);
                    }

                },
                (error) => {
                    alert(`Internal error! ${JSON.stringify(error)}`);
                    obs.next(false);
                }
            );
        });
    }


    private goBackBecauseWeGotGarbage(name: string, value: any) {
        const message = `we got garbage from router (name="${value}")`;
        this.goBack(message);
    }

    protected refreshData(id: number): Observable<boolean> {
        return new Observable<boolean>(
            (obs) => {
                this.service.read(id).subscribe(
                    (result) => {
                        this.dataToForm(result);
                        this.idSubject.next(this.id);
                        obs.next(true);
                    },
                    (err) => obs.error(err)
                );
            });
    }

    protected dataToForm(data: TEntityDetail): void {
        if (this.form && this.formWithConverters) {
            this.formWithConverters.patchValue(data);
            this.formEnabled = !this.isReadOnly;
        } else {
            let message = `${BaseEntityDetailComponent['name']}.dataToForm: `;
            if (!this.form) {
                message += 'this.form is not defined; ';
            }
            if (!this.formWithConverters) {
                message += 'this.formWithConverters is not defined';
            }
            throw new Error(message);
        }

    }

    private get entityAudit(): BaseEntityAudit {
        const isOperatorKnown = (this.userInfo && this.userInfo.user && this.userInfo.user.userName);
        const operator = isOperatorKnown ? this.userInfo.user.userName : 'UNKNOWN';
        return new BaseEntityAudit(operator);
    }

    protected audit() {
        if (this.form) {
            this.form.patchValue(this.entityAudit);
        }
    }

    protected applyAudit(
        entity: Partial<TEntityDetail> | Partial<TEntityForInsert> | Partial<TEntityForUpdate>
    ): Partial<TEntityDetail> | Partial<TEntityForInsert> | Partial<TEntityForUpdate> {
        return { ...entity, ...this.entityAudit };
    }

    protected getFormValue(controlName: string) {
        const control = this.form.get(controlName);
        if (control) {
            return control.value;
        }
    }

    public get auditOperator() {
        return this.getFormValue('operator');
    }
    public get auditRevision() {
        return this.getFormValue('revision');
    }

    protected clearServerErrorMap() {
        this.badRequestMap = undefined;
    }
    protected processServerErrorMap(brx?: BadRequestException) {
        if (brx) {
            this.badRequestMap = CasedObject.camelCase(brx.badRequestMap);
        } else {
            this.clearServerErrorMap();
        }
    }

    protected getFormValuesForInsert(
        rawValueList: (keyof TEntityForInsert)[],
        convertedValueList: (keyof TEntityForInsert)[],
        applyAuditInfo?: boolean
    ): Partial<TEntityForInsert> {
        return this.getFormValues<TEntityForInsert>(rawValueList, convertedValueList, applyAuditInfo);
    }
    protected getFormValuesForUpdate(
        rawValueList: (keyof TEntityForUpdate)[],
        convertedValueList: (keyof TEntityForUpdate)[],
        applyAuditInfo?: boolean
    ): Partial<TEntityForUpdate> {
        return this.getFormValues<TEntityForUpdate>(rawValueList, convertedValueList, applyAuditInfo);
    }
    private getFormValues<T>(
        rawValueList?: (keyof T)[],
        convertedValueList?: (keyof T)[],
        applyAuditInfo = true
    ): Partial<T> {
        const values: Partial<T> = {};
        const rvl = (rawValueList || []);
        const cvl = (convertedValueList || []);

        // Check no property appear in both arrays
        const emptyArray = rvl.filter((value) => cvl.includes(value));
        if (emptyArray.length > 0) {
            const [property, appears] = (emptyArray.length > 1) ? ['properties', 'appear'] : ['property', 'appears'];
            const pp = emptyArray.join(', ');
            throw new Error(`getFormValues: ${property} (${pp}) ${appears} in RAW list AND in CONVERTED list`);
        }
        // get raw values
        rvl.forEach((key) => {
            values[key] = this.getFormValue(key as string);
        });
        // get converted values
        cvl.forEach((key) => {
            values[key] = (this.formWithConverters.getValue(key as string) as any);
        });
        // apply audit info
        if (applyAuditInfo) {
            this.applyAudit(values as TEntityDetail);
        }

        return values;
    }

    public debugPrintInvalidControls() {
        if (isDevMode()) {
            const controls = this.form.controls;
            const validMap: { [key: string]: boolean } = {};
            let length = 0;
            for (const name in controls) {
                if (controls[name]) {
                    if (name.length > length) {
                        length = name.length;
                    }
                    validMap[name] = controls[name].invalid;
                }
            }
            for (const name in validMap) {
                const nameToPrint = name.padStart(length, ' ');
                if (validMap[name]) {
                    // tslint:disable-next-line
                    this.log.error(`- ${nameToPrint} is INVALID`);
                } else {
                    // tslint:disable-next-line
                    this.log.info(`- ${nameToPrint} is valid`);
                }
            }
        }
    }

    public get isDevMode(): boolean {
        return isDevMode();
    }
}
