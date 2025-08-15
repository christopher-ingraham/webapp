// Angular

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, APP_INITIALIZER, ErrorHandler, LOCALE_ID, Injector } from '@angular/core';
import { CommonModule } from '@angular/common';
import { registerLocaleData } from '@angular/common';
import localeIt from '@angular/common/locales/it';

// By default, Angular only contains locale data for en-US.
registerLocaleData(localeIt, 'it-IT');

// Other packages


import {
    AuthModule,
    AuthWellKnownEndpoints,
    OidcConfigService,
    OidcSecurityService,
    OpenIDImplicitFlowConfiguration,
} from 'angular-auth-oidc-client';

import 'hammerjs';

// Application

import {
    ApiService,
    ConfigurationService,
    CoreModule,
    GlobalErrorHandler,
    LogService,
} from '@app/core';
import { SharedModule } from '@app/shared';
import { ShellModule } from '@app/shell';
import {
    ApiTranslateLoader,
    TranslateHelperService,
    TranslateLoader,
    TranslateModule,
    TranslateParser,
    TranslateParserWithDefault,
    WidgetModule,
} from '@app/widget';

import { AppComponent } from './app-root';
import { PageNotFoundComponent } from './page-not-found.component';
import { PlaygroundComponent } from './playground/playground.component';

import { AppRoutingModule } from './app-routing.module';
import { ProgressBarModule } from '@progress/kendo-angular-progressbar';

import { environment } from '../environments/environment';
import { Router } from '@angular/router';
import { AuthInterceptor } from './shared/interceptor/auth-interceptor.class';


// reference: http://www.projectcodify.com/angular-set-base-url-dynamically
export function getBaseUrl() {
    const baseUrl = document.getElementsByTagName('base')[0].href;
    return baseUrl;
}


export function onAppInit(
    configurationService: ConfigurationService,
    oidcConfigService: OidcConfigService,
    logService: LogService

): () => Promise<any> {

    return () => {
        return configurationService.init().toPromise()
            .then((config) => {
                logService.info(`OidcConfigService configuration loading from STS ${config.stsUrl}...`);
                return oidcConfigService.load_using_stsServer(config.stsUrl);
            });
    };
}


@NgModule({
    declarations: [
        AppComponent,
        PageNotFoundComponent,
        PlaygroundComponent,
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        CommonModule,
        HttpClientModule,
        FormsModule,
        AppRoutingModule,
        AuthModule.forRoot(),
        CoreModule.forRoot(),
        SharedModule.forRoot(),
        ShellModule,

        BrowserAnimationsModule,

        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useClass: ApiTranslateLoader,
                deps: [ApiService]
            },
            parser: {
                provide: TranslateParser,
                useClass: TranslateParserWithDefault,
                deps: [TranslateHelperService]
            }
        }),

        WidgetModule.forRoot(),

        ProgressBarModule,
    ],
    providers: [
        DatePipe,
        OidcConfigService,
        {
            provide: HTTP_INTERCEPTORS,
            useFactory: function(router: Router, oidcSecurity: OidcSecurityService) {
              return new AuthInterceptor(router, oidcSecurity);
            },
            multi: true,
            deps: [Router, OidcSecurityService]
         },
        {
            provide: APP_INITIALIZER,
            useFactory: onAppInit,
            multi: true,
            deps: [ConfigurationService, OidcConfigService, LogService]
        },
        {
            provide: 'BASE_URL',
            useFactory: getBaseUrl
        },
        {
            provide: ErrorHandler,
            useClass: GlobalErrorHandler,
            deps: [Injector]
        },
        {
            provide: LOCALE_ID,
            useValue: 'en-US'
        },
    ],
    bootstrap: [AppComponent]
})
export class AppModule {

    constructor(
        private oidcSecurityService: OidcSecurityService,
        private oidcConfigService: OidcConfigService,
        private config: ConfigurationService,
        private log: LogService
    ) {

        this.oidcConfigService.onConfigurationLoaded.subscribe(
            (configResult: boolean) => {

                this.log.info(`configuration loaded! (configResult=${configResult})`);

                const oidcConfig = new OpenIDImplicitFlowConfiguration();
                const configuration = this.config.get();
                const stsConfig = configuration.stsClient;

                oidcConfig.stsServer = configuration.stsUrl;
                oidcConfig.redirect_url = stsConfig.redirectUrl;
                oidcConfig.post_logout_redirect_uri = stsConfig.postLogoutRedirectUri;
                oidcConfig.client_id = stsConfig.clientId;
                oidcConfig.scope = stsConfig.scope;
                oidcConfig.response_type = stsConfig.responseType;

                oidcConfig.silent_renew = stsConfig.silentRenew;
                oidcConfig.silent_renew_url = stsConfig.silentRenewUrl;

                // Authentication is slow to start the application and this causes a timeout checking
                // 'iat' (issued at) timestamp of the id token during authentication flow:
                // the issued token is validated after the canonical 3 seconds of the timeout
                // and this causes a token validation error
                oidcConfig.max_id_token_iat_offset_allowed_in_seconds = 3; // default timeout
                if (stsConfig.oAuthTokenValidationTimeOut) { // Value overwritten by custom value
                    oidcConfig.max_id_token_iat_offset_allowed_in_seconds = +stsConfig.oAuthTokenValidationTimeOut;
                }

                oidcConfig.max_id_token_iat_offset_allowed_in_seconds = 600;
                oidcConfig.log_console_warning_active = true;
                oidcConfig.log_console_debug_active = !environment.production;

                const authWellKnownEndpoints = new AuthWellKnownEndpoints();
                authWellKnownEndpoints.setWellKnownEndpoints(this.oidcConfigService.wellKnownEndpoints);

                this.oidcSecurityService.setupModule(oidcConfig, authWellKnownEndpoints);
            }
        );
    }
}

