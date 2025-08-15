import { Component, OnInit, OnDestroy } from '@angular/core';

import {
    AppUserInfoService,
    FooterInfo,
    NotifierService,
    NotificationAppState,
} from '@app/core';
import { SubscriptionList } from '@app/shared';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html'
})
export class FooterComponent extends SubscriptionList implements OnInit, OnDestroy {

    private footerInfo: FooterInfo;
    private appState: NotificationAppState = NotificationAppState.Ready;

    public get isPresent(): boolean {
        return this.footerInfo && this.footerInfo.isVisible;
    }

    public get textKey(): string {
        return this.footerInfo ? this.footerInfo.textKey : '';
    }

    public get isReady(): boolean {
        return (this.appState === NotificationAppState.Ready);
    }

    public get isBusy(): boolean {
        return (this.appState === NotificationAppState.Busy);
    }

    public get state(): string {
        return `SHELL.FOOTER.APP_STATE.${NotificationAppState[this.appState]}`.toUpperCase();
    }

    constructor(
        private appUserInfo: AppUserInfoService,
        private notifier: NotifierService
    ) {
        super();
    }

    ngOnInit(): void {

        this.subscribe(
            this.appUserInfo.get().subscribe((appUserInfo) => {
                this.footerInfo =
                    appUserInfo
                    && appUserInfo.application
                    && appUserInfo.application.footer;
            }),
            this.notifier.getAppState().subscribe(newAppState => {
                this.appState = newAppState;
            })
        );
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }

}
