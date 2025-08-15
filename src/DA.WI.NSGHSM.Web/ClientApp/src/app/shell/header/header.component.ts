import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { AppUserInfoService, ApplicationInfo } from '@app/core';
import { SubscriptionList } from '@app/shared';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html'
})
export class HeaderComponent extends SubscriptionList implements OnInit, OnDestroy {

    applicationInfo: ApplicationInfo;

    constructor(
        private appUserInfo: AppUserInfoService
    ) {
        super();
    }

    ngOnInit(): void {

        this.subscribe(
            this.appUserInfo.get()
                .subscribe(appUserInfo => {

                    this.applicationInfo =
                        appUserInfo
                        && appUserInfo.application;
                })
        );
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }
}
