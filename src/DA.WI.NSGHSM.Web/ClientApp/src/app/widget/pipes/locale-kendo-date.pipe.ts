import { Pipe, PipeTransform, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { IntlService, DatePipe } from '@progress/kendo-angular-intl';
import { Subscription } from 'rxjs';

@Pipe({
  name: 'localeKendoDate',
  pure: false
})
export class LocaleKendoDatePipe implements PipeTransform, OnDestroy {

  cachedValue: any;
  flag: boolean = true;
  subscriptions: Subscription;

  constructor(private datePipe: DatePipe,
              private intlService: IntlService) {
              this.subscriptions = this.intlService.changes.subscribe(() => this.flag = true);
  }

  transform(value: any, args?: any): any {
    if (this.flag === true) {
      this.cachedValue = this.datePipe.transform(value, args);
      this.flag = false;
    }

    return this.cachedValue;
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
