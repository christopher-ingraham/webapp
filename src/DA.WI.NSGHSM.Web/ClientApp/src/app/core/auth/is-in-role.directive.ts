import { Directive, TemplateRef, ViewContainerRef, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from './auth.service';
import { LogService } from '../log/log.service';

@Directive({
  selector: '[appIsInRole]'
})
export class IsInRoleDirective implements OnDestroy {

  subscription: Subscription;
  private hasView = false;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private auth: AuthService,
    private log: LogService
  ) { }

  @Input() set isInRole(requestedRoles: string[]) {

    this.log.debug(`IsInRoleDirective - waiting for isInRole result...`, requestedRoles);

    this.subscription = this.auth.isInRole(requestedRoles)
      .subscribe(isInRole => {
        this.log.debug(`IsInRoleDirective - result: ${isInRole}`);

        if (isInRole && !this.hasView) {

          this.viewContainer.createEmbeddedView(this.templateRef);
          this.hasView = true;
        } else if (!isInRole && this.hasView) {

          this.viewContainer.clear();
          this.hasView = false;
        }
      });
  }


  ngOnDestroy(): void {

    this.log.debug(`IsInRoleDirective - unsuscribed`);

    this.subscription.unsubscribe();
  }

}
