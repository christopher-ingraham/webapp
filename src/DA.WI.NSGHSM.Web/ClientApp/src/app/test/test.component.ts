import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subscription } from 'rxjs';

import { TranslateService } from '@ngx-translate/core';
import { IntlService, CldrIntlService } from '@progress/kendo-angular-intl';

import { LocaleService, LocaleHelperService } from '@app/widget';

import {
    ApiService,
    AppUserInfoService,
    AuthService,
    ErrorDecoderService,
    Locale,
    NotificationEntryLevel,
    NotifierService,
    Roles,
} from '@app/core';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styles: ['pre { white-space: pre-wrap; }']
})
export class TestComponent implements OnInit, OnDestroy {

  NotificationEntryLevel = NotificationEntryLevel;

  public number = 42.123;
  public date = new Date();
  public localeId: string;
  availableLocales: Array<Locale> = [];
  currentLocale: Locale;
  private subscriptions: Subscription[] = [];

  constructor(
    private api: ApiService,
    private notifier: NotifierService,
    private appUserInfo: AppUserInfoService,
    private authService: AuthService,
    private errorDecoderService: ErrorDecoderService,
    private translateService: TranslateService,
    private localeService: LocaleService,
    public intlService: IntlService,
    private localeHelperService: LocaleHelperService
  ) { }

  public get isCreateUserEnabled(): Observable<boolean> {
    return this.authService.isInRole([Roles.All]);
  }

  public translationFoundResult: string = '-';
  public translationWithParameterFoundResult: string = '-';
  public translationNotFoundResult: string = '-';
  public translationWithParameterNotFoundResult: string = '-';
  apiPingResult: string[] = [];
  apiGetUsersResult: string[] = [];
  apiPostUserName: string = '';
  apiPostUserResult: string[] = [];

  public reloadTranslationFound(key: string): void {

    this.translationFoundResult = this.translateService.instant(key);
  }

  public reloadTranslationWithParameterFound(key: string, param: Object): void {

    this.translationWithParameterFoundResult = this.translateService.instant(key, param);
  }

  public reloadTranslationNotFound(key: string): void {

    this.translationNotFoundResult = this.translateService.instant(key);
  }

  public reloadTranslationWithParameterNotFound(key: string, param: Object): void {

    this.translationWithParameterNotFoundResult = this.translateService.instant(key, param);
  }

  ngOnInit() {

    const _this = this;

    const appUserInfo = this.appUserInfo.get();

    // This change the Country localization inside Kendo Controls
    this.localeId = (<CldrIntlService>this.intlService).localeId;

    this.subscriptions.push(
      appUserInfo
        .subscribe(appUsrInfo => {
          _this.availableLocales =
            appUsrInfo
            && appUsrInfo.application
            && this.localeHelperService.toArray(appUsrInfo.application.locales);
          _this.currentLocale = _this.availableLocales && _this.availableLocales.filter(_ => _.code === _this.localeService.currentCode)[0];
        }));

  }

  apiPing() {

    this.apiPingResult = [];
    this.apiGet('test/ping', this.apiPingResult);
  }

  apiGetUsers() {

    this.apiGetUsersResult = [];
    this.apiGet('user', this.apiGetUsersResult);
  }

  apiPostUser() {
    this.apiPostUserResult = [];
    this.apiPost('user', { userName: this.apiPostUserName }, this.apiPostUserResult);
  }

  postNotification(notificationLevel: NotificationEntryLevel) {

    switch (notificationLevel) {

      case NotificationEntryLevel.Error: this.notifier.error(NotificationEntryLevel[notificationLevel]); break;
      case NotificationEntryLevel.Info: this.notifier.info(NotificationEntryLevel[notificationLevel]); break;
      case NotificationEntryLevel.Warning: this.notifier.warning(NotificationEntryLevel[notificationLevel]); break;
      case NotificationEntryLevel.Success: this.notifier.success(NotificationEntryLevel[notificationLevel]); break;
    }
  }

  private apiPost(url: string, data: any, feedbackEntries: string[]) {

    feedbackEntries.push(`Calling POST ${url} with body ${JSON.stringify(data)}...`);
    this.api
      .post<any>(url, data)
      .subscribe(
        result => this.handleSuccess(result, feedbackEntries),
        error => this.handleError(error, feedbackEntries)
      );
  }


  private apiGet(url: string, feedbackEntries: string[]) {

    feedbackEntries.push(`Calling GET ${url}...`);
    this.api
      .get<any>(url)
      .subscribe(
        result => this.handleSuccess(result, feedbackEntries),
        failure => this.handleError(failure, feedbackEntries)
      );
  }

  private handleSuccess(result: any, feedbackEntries: string[]): void {

    const message = 'Api responded with success';
    feedbackEntries.push(`${message} ${JSON.stringify(result)}`);

    this.notifier.success(message);
  }

  private handleError(error: any, feedbackEntries: string[]): void {

    const message = `Api responded with error: ${JSON.stringify(error)}; Decoded error message: ${this.errorDecoderService.decode(error)}`;
    feedbackEntries.push(message);

    this.notifier.error(message);
  }

  public onLocaleChange(locale: Locale): void {
    this.localeService.use(locale.code);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(_ => _.unsubscribe());
  }

}
