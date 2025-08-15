import { isDevMode, OnDestroy, ViewChild, OnInit } from '@angular/core';

import { TabStripComponent } from '@app/widget';

import { SubscriptionList } from '../utility/subscription-list.class';
import { TabstripMetadata } from '../model/tabstrip-metadata.class';
import { AuthService } from '@app/core';

export class BaseMasterDetailTabbedComponent
    extends SubscriptionList
    implements OnDestroy, OnInit {

    public readonly modelOptions = { standalone: true };
    public readonly numberFormatOptions = { maximumFractionDigits: 0 };

    public readonly tabstrip: TabstripMetadata;

    protected isInAllRole = false;
    protected isInRestrictedRole = false;
    protected isReadOnly = false;

    @ViewChild(TabStripComponent, { static: false })
    protected parentTabComponent: TabStripComponent;

    constructor(
        private authService: AuthService,
        title: string = 'No title',
    ) {
        super();
        this.tabstrip = new TabstripMetadata(title);
    }

    ngOnDestroy() {
        this.unsubscribeAll();
    }

    ngOnInit() {
        this.tabstrip.tabSelected.subscribe((tabIndex) => {
            // process programmatic tab selection requests
            this.parentTabComponent.selectTab(tabIndex);
        });
    }

    public onSelectTab(tabIndex: number) {
        this.tabstrip.selected = tabIndex;
    }

    public get isDevMode(): boolean {
        return isDevMode();
    }

    protected setupSecurityRoleFlags() {
        const authService = this.authService;
        this.subscribe(
            authService.isInAllRole.subscribe((isInRole) => this.isInAllRole = isInRole),
            authService.isInRestrictedRole.subscribe((isInRole) => this.isInRestrictedRole = isInRole),
            authService.isInReadOnlyRole.subscribe((isInRole) => this.isReadOnly = isInRole),
        );
    }
    protected dumpSecurityRoleFlags() {
        if (this.isDevMode) {
            const dot = (flag: boolean) => flag ? '●' : '○';
            const roles: string[] = [
                '┌──────────────────',
                `│        ALL ${dot(this.isInAllRole)}`,
                `│ RESTRICTED ${dot(this.isInRestrictedRole)}`,
                `│  READ ONLY ${dot(this.isReadOnly)}`,
                '└──────────────────',
            ];
            // tslint:disable-next-line
            console.log(roles.join('\n'));
        }
    }
}
