import { Injectable, OnDestroy, LOCALE_ID, Inject } from '@angular/core';

import { Observable, Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { TranslateService } from '@ngx-translate/core';

import { MessageService } from '@progress/kendo-angular-l10n';
import { IntlService, CldrIntlService } from '@progress/kendo-angular-intl';

import { AppUserInfoService, AppUserInfo, Locale } from '@app/core';
import { LocaleHelperService } from '../helpers';

@Injectable({
    providedIn: 'root'
})
export class LocaleService implements OnDestroy {

    private subscriptions: Subscription[] = [];

    private _currentCode: string;
    public get currentCode(): string {
        return this._currentCode;
    }
    public set currentCode(value: string) {
        this._currentCode = value;
    }

    constructor(private translateService: TranslateService,
        private messageService: MessageService,
        private appUserInfoService: AppUserInfoService,
        private intlService: IntlService,
        private localeHelperService: LocaleHelperService
    ) {
    }

    public init(): Observable<any> {
        return this.appUserInfoService.get()
            .pipe(
                switchMap((appInfo: AppUserInfo) => {

                    const locales = (appInfo && appInfo.application.locales);
                    const defaultLocale: Locale = this.localeHelperService.toArray(locales).filter(_ => _.isDefault === true)[0];

                    this.setDefaultLang(defaultLocale.code);
                    return this.use(defaultLocale.code);
                }));
    }

    // in this function the code parameter is the complete language-Country code (es: es-US)
    public use(code: string): Observable<any> {

        // Extract the LanguageCode from complete language-Country code (es: en-US => en)
        const localeCode = this.localeHelperService.getLanguageCode(code);

        const tmp = this.translateService.use(localeCode);

        // This change the Country localization inside Kendo Controls
        (<CldrIntlService>this.intlService).localeId = code;

        this.currentCode = code;

        this.subscriptions.push(tmp.subscribe(() => {
            this.messageService.notify(); // force the refresh of kendo controls language
        }));

        return tmp;
    }

    public setDefaultLang(code: string): void {
        return this.translateService.setDefaultLang(this.localeHelperService.getLanguageCode(code));
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(_ => _.unsubscribe());
    }
}
