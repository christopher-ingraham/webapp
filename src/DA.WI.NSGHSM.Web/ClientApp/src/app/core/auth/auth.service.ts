import { Injectable, isDevMode } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

import { OidcSecurityService } from 'angular-auth-oidc-client';

import { Roles } from '../application-constants';

import { environment } from '@app/environments';

import { AppUserInfo, AppUserInfoService } from '../app-user-info';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    constructor(
        private oidcSecurityService: OidcSecurityService,
        private appUserInfoService: AppUserInfoService
    ) { }

    public isInRole(requestedRoles: string[] = []): Observable<boolean> {

        const isInRoleObservable = this.oidcSecurityService.getIsAuthorized()
            .pipe(
                switchMap((isAuthorized) => {

                    return isAuthorized
                        ? this.appUserInfoService.get()
                        : of(<AppUserInfo>null);
                }),
                map(appUserInfo => this.isInRoleInstant(appUserInfo, requestedRoles))
            );

        return isInRoleObservable;
    }

    public isInRoleInstant(appUserInfo: AppUserInfo, roles: string[]): boolean {

        if (appUserInfo && appUserInfo.user && appUserInfo.user.roles) {
            const isUserInAnyRole = () => {
                const userOwnedRoles = appUserInfo.user.roles;
                const foundRoles = roles ? roles.filter((role) => (userOwnedRoles.indexOf(role) >= 0)) : [];
                // This implementation holds true if any role out of "roles" is present in logged-in user's role list
                return (foundRoles.length > 0);
            };

            if (environment.production) {
                return isUserInAnyRole();
            } else {
                // DEVELOPMENT MODE ONLY
                if (environment.bypassAuthorization) {
                    return true;
                } else {
                    return isUserInAnyRole();
                }
            }
        } else {
            return false;
        }

    }

    public get isInAllRole(): Observable<boolean> {
        return this.isInRole([Roles.All]);
    }
    public get isInRestrictedRole(): Observable<boolean> {
        return this.isInRole([Roles.Restricted]);
    }
    public get isInReadOnlyRole(): Observable<boolean> {
        return this.isInRole([Roles.ReadOnly]);
    }
}
