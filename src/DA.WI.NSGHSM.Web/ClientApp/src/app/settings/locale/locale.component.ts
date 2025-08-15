import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { AppUserInfoService, Locale } from '@app/core';
import { AppStatusStoreService, SystemOfUnitsInfo } from '@app/shared';
import { LocaleService, LocaleHelperService } from '@app/widget';

@Component({
    selector: 'app-locale',
    templateUrl: './locale.component.html'
})
export class LocaleComponent implements OnInit, OnDestroy {

    availableLocales: Array<Locale> = [];
    currentLocale: Locale;

    public availableSystemOfUnits: SystemOfUnitsInfo[] = [
        // https://simple.wikipedia.org/wiki/International_System_of_Units
        new SystemOfUnitsInfo(SystemOfUnitsInfo.souSI, 'SI: International System of Units'),
        // https://simple.wikipedia.org/wiki/United_States_customary_units
        new SystemOfUnitsInfo(SystemOfUnitsInfo.souUSCS, 'USCS: United States customary units'),
    ];
    public get currentSystemOfUnits(): SystemOfUnitsInfo {
        if (this.appStatus.isUomSI) {
            return this.availableSystemOfUnits[0];
        } else {
            return this.availableSystemOfUnits[1];
        }
    }

    private subscriptions: Subscription[] = [];

    constructor(private appUserInfo: AppUserInfoService,
        private localeService: LocaleService,
        private localeHelperService: LocaleHelperService,
        private appStatus: AppStatusStoreService
    ) { }

    ngOnInit() {

        const _this = this;

        this.subscriptions.push(
            this.appUserInfo.get()
                .subscribe(appUserInfo => {
                    _this.availableLocales =
                        appUserInfo
                        && appUserInfo.application
                        && this.localeHelperService.toArray(appUserInfo.application.locales);
                    _this.currentLocale = _this.availableLocales &&
                        _this.availableLocales.filter(_ => _.code === _this.localeService.currentCode)[0];
                }));

    }

    onLocaleChange(lang: Locale): void {

        this.localeService.use(lang.code);
        this.currentLocale = lang;
    }

    public onSystemOfUnitsChange(value: SystemOfUnitsInfo) {
        this.appStatus.isUomSI = value.isSI;
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach((subscription) => subscription.unsubscribe());
    }

}
