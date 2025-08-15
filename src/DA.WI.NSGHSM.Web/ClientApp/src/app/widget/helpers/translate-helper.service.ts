import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TranslateHelperService {

  constructor() { }

  public parseKeyWithDefault(keyWithDefault: string): {key: string, default: string} {

    if (keyWithDefault !== null) {

      const position: number = keyWithDefault.indexOf('|');
      if (position !== -1) {

        const _key: string = keyWithDefault.substring(0, position);
        let _default: string = keyWithDefault.substring(position + 1);
        _default = _default
          .split('[[').join('{{')
          .split(']]').join('}}');

        return {key: _key, default: _default};
      }
    }

    return { key: keyWithDefault, default: null };
  }
}
