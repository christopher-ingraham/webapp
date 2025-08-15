import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

// Application

import { UnauthorizedComponent } from './auth/unauthorized/unauthorized.component';

import { ApiService } from './api';
import { AppUserInfoService } from './app-user-info';
import { AuthGuard, AuthService } from './auth';
import { ConfigurationService } from './configuration';
import { ErrorDecoderService } from './error';
import { ValidationsDecoderService } from './errors';
import { JsonHelperService, UrlHelperService } from './helpers';
import { LogService } from './log';
import { NotifierService } from './notifier';

import { IsInRoleDirective } from './auth';



const components = [
    UnauthorizedComponent,
];

const modules = [
    CommonModule,
    RouterModule,
];

const directives = [
    IsInRoleDirective,
];

const guards = [
    AuthGuard,
];

const services = [
    ApiService,
    AppUserInfoService,
    AuthService,
    ConfigurationService,
    ErrorDecoderService,
    JsonHelperService,
    LogService,
    NotifierService,
    UrlHelperService,
    ValidationsDecoderService,
];

const providers = [
    ...services,
    ...guards,
];

@NgModule({
    declarations: [
        ...components,
        ...directives,
    ],
    imports: [
        ...modules,
    ],
    exports: [
        ...modules,
        ...directives,
    ]
})
export class CoreModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: CoreModule,
            providers,
        };
    }
}
