import { Injectable } from '@angular/core';

import { Locale } from '@app/core';

@Injectable({
  providedIn: 'root'
})
export class LocaleHelperService {

  public getLanguageCode(code: string): string {
    if (!code || !code.match('^[a-z]{2}-[A-Z]{2}$')) {
      return null;
    }
    return code.split('-')[0];
  }

  public getCountryCode(code: string): string {
    if (!code || !code.match('^[a-z]{2}-[A-Z]{2}$')) {
      return null;
    }
    return code.split('-')[1];
  }


  public toArray(localeDictionary: { [code: string]: Locale; }): Array<Locale> {

    const returnedArray = new Array<Locale>();

    if (!localeDictionary) {
      return [];
    }

    Object.entries(localeDictionary).forEach(el => {
      returnedArray.push({ code: el[0], description: el[1].description, isDefault: el[1].isDefault });
    });

    return returnedArray;

  }

}
