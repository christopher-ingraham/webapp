import { MessageService } from '@progress/kendo-angular-l10n';
import { TranslateService } from '@ngx-translate/core';

export class KendoTranslateService extends MessageService {
  constructor(private translateService: TranslateService) {
    super();
  }

  public get(key: string): string {
    const translated: string = this.translateService.instant(key.toUpperCase());

    if (translated === key.toUpperCase()) {
      return undefined;
    }

    return translated;
  }
}
