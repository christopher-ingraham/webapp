import { Component, OnInit, OnDestroy } from '@angular/core';

import { OidcSecurityService } from 'angular-auth-oidc-client';

import { User, AppUserInfoService } from '@app/core';
import { SubscriptionList } from '@app/shared';

@Component({
    selector: 'app-user',
    templateUrl: './user.component.html'
})
export class UserComponent
    extends SubscriptionList
    implements OnInit, OnDestroy {

    private user: User;

    public get userName(): string {
        const unknownUserName = 'UNKNOWN';
        if (this.user) {
            return this.user.userName;
        } else {
            return unknownUserName;
        }
    }

    public get userRoles(): string {
        if (this.user && this.user.roles) {
            return this.user.roles.join(', ');
        } else {
            return '';
        }
    }

    constructor(
        private appUserInfoService: AppUserInfoService,
        private oidcSecurity: OidcSecurityService
    ) {
        super();
    }

    ngOnInit(): void {
        this.subscribe(
            this.appUserInfoService.get().subscribe(
                (appUserInfo) => this.user = appUserInfo.user
            )
        );
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }

    onLogoutClicked() {
        this.oidcSecurity.logoff();
    }

}
