import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription, Observable } from 'rxjs';
import { tap, filter, switchMap, take, catchError } from 'rxjs/operators';

import { OidcSecurityService } from 'angular-auth-oidc-client';

import { LocaleService } from '@app/widget';

import { AppUserInfoService, LogService, } from '@app/core';

import { environment } from '../../environments/environment';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {

    get isInitialized(): boolean {

        return this.isAuthenticationInitialized
            && this.isLocaleInitialized;
    }

    isAuthenticationInitialized: boolean = false;
    isLocaleInitialized: boolean = false;
    initStateMessages: string[] = [];
    initErrorMessage: string;

    private subscriptions: Subscription[] = [];

    constructor(
        private oidcSecurity: OidcSecurityService,
        private appUserInfoService: AppUserInfoService,
        private log: LogService,
        private localeService: LocaleService
    ) {

    }

    ngOnInit(): void {
        if (environment.bypassAuthorization) {
            this.subscriptions.push(this.initAuthentication());
            this.appUserInfoService.init().subscribe((appUserInfo) => {
                this.isAuthenticationInitialized = true;
                this.initStateMessages.push(`User "${appUserInfo.user.userName}" logged in!`);
            });
            this.subscriptions.push(this.initLocale());
        } else {
            this.setupAuthentication();
            this.subscriptions.push(
                this.initAuthentication(),
                this.initLocale()
            );
        }
        this.subscriptions.push(this.initLocale());
    }

    ngOnDestroy(): void {

        this.subscriptions.forEach(_ => _.unsubscribe());
    }

    private setupAuthentication() {

        if (this.oidcSecurity.moduleSetup) {
            this.doCallbackLogicIfRequired();
        } else {
            this.subscriptions.push(this.oidcSecurity.onModuleSetup.subscribe(() => {
                this.doCallbackLogicIfRequired();
            }));
        }
    }

    private initAuthentication(): Subscription {


        this.initStateMessages.push('Logging in...');
        return this.oidcSecurity.getIsAuthorized()
            .pipe(
                filter((isAuthorized) => isAuthorized),
                take(1),
                switchMap(() => this.appUserInfoService.init()),
                catchError((err) => this.handleInitializationError(err))
            )
            .subscribe(
                () => {
                    this.isAuthenticationInitialized = true;
                    this.initStateMessages.push('Logged in!');
                },
                (error) => {
                    this.isAuthenticationInitialized = false;
                    this.initStateMessages.push('Log in failed!');
                });
    }

    private initLocale(): Subscription {
        return this.localeService.init()
            .subscribe(() => {
                this.isLocaleInitialized = true;
                this.initStateMessages.push('Locale initialized!');
            });
    }

    private handleInitializationError(err: any): Observable<any> {

        this.initErrorMessage = `An error occurred: ${err.message || err}.`;

        return err;
    }

    private doCallbackLogicIfRequired(): any {

        this.log.info('doCallbackLogicIfRequired started', window.location.hash);
        if (window.location.hash) {

            this.oidcSecurity.authorizedImplicitFlowCallback();
        } else {

            // autologin
            this.subscriptions.push(
                this.oidcSecurity.getIsAuthorized()
                    .pipe(
                        filter(isAuthorized => !isAuthorized),
                        take(1),
                        tap(() => this.oidcSecurity.authorize())
                    )
                    .subscribe()
            );
        }
    }
}
