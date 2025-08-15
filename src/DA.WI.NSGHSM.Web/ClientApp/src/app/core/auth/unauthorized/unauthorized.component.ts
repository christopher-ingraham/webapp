import { Component, OnInit, OnDestroy } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html'
})
export class UnauthorizedComponent implements OnInit, OnDestroy {

  isAuthorized: boolean;
  subscription: Subscription;

  constructor(private oidcSecurityService: OidcSecurityService) { }

  ngOnInit(): void {

    this.subscription = this.oidcSecurityService
      .getIsAuthorized()
      .subscribe(isAuthorized => this.isAuthorized = isAuthorized);
  }

  ngOnDestroy(): void {

    this.subscription.unsubscribe();
  }

  login() {

    this.oidcSecurityService.authorize();
  }

  logout() {

    this.oidcSecurityService.logoff();
  }

}
