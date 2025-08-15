import { Pipe, PipeTransform, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { NumberPipe, IntlService } from '@progress/kendo-angular-intl';
import { Subscription } from 'rxjs';

@Pipe({
  name: 'localeKendoNumber',
  pure: false
})
export class LocaleKendoNumberPipe implements PipeTransform, OnDestroy {

  cachedValue: any;
  flag: boolean = true;
  subscriptions: Subscription;

  constructor(private numberPipe: NumberPipe,
              private intlService: IntlService) {
              this.subscriptions = this.intlService.changes.subscribe(() => this.flag = true);
  }

  transform(value: any, args?: any): any {
    if (this.flag === true) {
      this.cachedValue = this.numberPipe.transform(value, args);
      this.flag = false;
    }

    return this.cachedValue;
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
