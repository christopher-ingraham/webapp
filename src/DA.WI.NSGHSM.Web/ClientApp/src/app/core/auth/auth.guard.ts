import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { AuthService } from './auth.service';
import { AppUserInfo, MenuInfo, AppUserInfoService } from '../app-user-info';
import { UrlHelperService } from '../helpers';

import { environment } from '../../../environments/environment';


@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    constructor(
        private router: Router,
        private auth: AuthService,
        private appUserInfoService: AppUserInfoService,
        private urlHelper: UrlHelperService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
        : boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

        if (environment.bypassAuthorization) {
            return true;
        } else {
            return this.appUserInfoService.get()
                .pipe(
                    map((appUserInfo: AppUserInfo) => {
                        const roles: string[] = this.getRequestedRoles(appUserInfo, route, state);
                        return this.auth.isInRoleInstant(appUserInfo, roles);
                    }),
                    tap((isInRole: boolean) => {
                        if (!isInRole) {
                            // route '/unauthorized' is defined in AppRoutingModule
                            // da-nsghsm/DA.WI.NSGHSM.Web/ClientApp/src/app/app-routing.module.ts
                            this.router.navigate(['/unauthorized']);
                        }
                    })
                );
        }
    }

    private getRequestedRoles(
        appUserInfo: AppUserInfo,
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): string[] {

        if (route.data && route.data.roles) {
            return route.data.roles;
        }

        if (appUserInfo.application.menu) {
            const menuInfo = this.FindMenuItem(appUserInfo.application.menu, state.url);

            if (menuInfo && menuInfo.roles) {
                return menuInfo.roles;
            }
        }

        return [];
    }

    private FindMenuItem(menuItems: MenuInfo[], path: string) {

        const matches = this.FindMenuItems(menuItems, path);

        if (!matches) {
            return null;
        }

        if (matches.length > 1) {
            throw new Error('Two menu items have the same path');
        }

        return matches[0];
    }

    private FindMenuItems(menuItems: MenuInfo[], path: string): MenuInfo[] {

        return menuItems.reduce((acc, next) => acc.concat(this.FindMenuItemsRecursively(next, path)), []);
    }

    private FindMenuItemsRecursively(menuItem: MenuInfo, path: string): MenuInfo[] {

        const matches: MenuInfo[] = this.MenuItemMatches(menuItem, path) ? [menuItem] : [];

        if (!menuItem.children || !menuItem.children.length) {
            return matches;
        }

        return menuItem.children.reduce((acc, next) => acc.concat(this.FindMenuItemsRecursively(next, path)),
            matches);
    }

    private MenuItemMatches(menuItem: MenuInfo, path: string): boolean {

        return this.urlHelper.trimLeadingSlash(menuItem.path) === this.urlHelper.trimLeadingSlash(path);
    }
}
