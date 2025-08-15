import { Component, EventEmitter, Output, OnInit, OnDestroy, isDevMode } from '@angular/core';
import { Router } from '@angular/router';

import { TranslateService } from '@ngx-translate/core';
import { PanelBarItemModel } from '@progress/kendo-angular-layout';

import {
    AppUserInfo,
    AppUserInfoService,
    AuthService,
    MenuInfo,
    Roles,
} from '@app/core';
import { SubscriptionList } from '@app/shared';

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html'
})
export class SidebarComponent extends SubscriptionList implements OnInit, OnDestroy {
    @Output() linkClicked = new EventEmitter();

    private roles = Roles;
    public panelBarItems: PanelBarItemModel[] = [];
    public appUserInfo: AppUserInfo;
    public appUserInfoVisible = false;

    private noRoutePrefix = '>noRoute<'; // This is used as No Routing on PanelBar Item

    public get isDebug(): boolean {
        return isDevMode();
    }

    constructor(
        private authService: AuthService,
        private router: Router,
        private appUserInfoService: AppUserInfoService,
        private translateService: TranslateService
    ) {
        super();
    }

    ngOnInit(): void {

        this.subscribe(
            this.appUserInfoService.get()
                .subscribe((appUserInfo) => {
                    this.panelBarItems = this.fillPanelBarItemFromAppUserInfo(appUserInfo);
                    this.appUserInfo = appUserInfo;
                })
        );

        this.subscribe(
            this.translateService.onLangChange.subscribe(() => {
                if (this.appUserInfo) {
                    this.panelBarItems = this.fillPanelBarItemFromAppUserInfo(this.appUserInfo);
                }
            })
        );
    }


    public stateChange(data: Array<PanelBarItemModel>): boolean {
        const focusedEvent: PanelBarItemModel = data.filter(item => item.focused === true)[0];

        // id=="" means no routing
        if (!focusedEvent || !focusedEvent.id || focusedEvent.id.startsWith(this.noRoutePrefix)) {
            return false;
        }

        this.router.navigate([focusedEvent.id]);
        this.linkClicked.emit();

        return false;
    }


    private fillPanelBarItemFromAppUserInfo(appUserInfo: AppUserInfo): PanelBarItemModel[] {
        const noRouteCounter: number[] = [0];

        if (appUserInfo && appUserInfo.application && appUserInfo.application.menu) {
            return this.fillPanelBarItemFromMenu(appUserInfo.application.menu, appUserInfo, noRouteCounter);
        }

        return [];
    }

    private fillPanelBarItemFromMenu(
        menu: MenuInfo[],
        appUserInfo: AppUserInfo,
        noRouteCounter: number[]
    ): PanelBarItemModel[] | undefined {

        if (menu) {
            return menu
                .filter((menuItem) => this.authService.isInRoleInstant(appUserInfo, menuItem.roles))
                .map((menuItem) => {
                    let title = this.translateService.instant(menuItem.key);
                    if (title === menuItem.key) {
                        title = menuItem.default;
                    }

                    const panelBarItemModel: Partial<PanelBarItemModel> = {
                        children: this.fillPanelBarItemFromMenu(menuItem.children, appUserInfo, noRouteCounter),
                        expanded: false,
                        icon: menuItem.icon,
                        id: menuItem.path || `${this.noRoutePrefix}.${noRouteCounter[0]++}`,
                        title,
                    };

                    return panelBarItemModel as PanelBarItemModel;
                });
        }
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }

}
