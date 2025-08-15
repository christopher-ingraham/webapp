import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { ApplicationInfo, AppUserInfoService } from '@app/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html'
})
export class AboutComponent implements OnInit, OnDestroy {

  applicationInfo: ApplicationInfo;

  private subscriptions: Subscription[] = [];

  constructor(
    private appUserInfo: AppUserInfoService) { }

  ngOnInit(): void {

    this.subscriptions.push(
        this.appUserInfo.get()
          .subscribe(appUserInfo => {

            this.applicationInfo =
              appUserInfo
              && appUserInfo.application;
          })
      );

    // this.subscriptions.push(
    //   this.appUserInfoService
    //     .get()
    //     .subscribe(_ => this.applicationInfo = _.application)
    // );
  }

  ngOnDestroy(): void {

    this.subscriptions.forEach(_ => _.unsubscribe());
  }

}
