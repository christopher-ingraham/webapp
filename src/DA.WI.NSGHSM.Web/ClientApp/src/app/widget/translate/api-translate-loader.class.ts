import { TranslateLoader } from '@ngx-translate/core';

import { Observable } from 'rxjs';

import { ApiService } from '@app/core';

export class ApiTranslateLoader implements TranslateLoader {

    constructor(private api: ApiService) {
    }

    public getTranslation(lang: String): Observable<any> {
        return this.api.get(`locale/${lang}`);
    }

}
