import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

import { AppUserInfo } from './model';
import { ApiService } from '../api';
import { LogService } from '../log';

@Injectable({
    providedIn: 'root'
})
export class AppUserInfoService {
    private readonly className: string;
    private readonly appUserInfoSubject: ReplaySubject<AppUserInfo>;

    constructor(
        private api: ApiService,
        private log: LogService
    ) {
        this.className = AppUserInfoService['name'];
        this.appUserInfoSubject = new ReplaySubject<AppUserInfo>();
    }

    public init(): Observable<AppUserInfo> {

        this.log.info(`initializing ${this.className}...`);

        return this.api.get<AppUserInfo>('appuserinfo')
            .pipe(
                tap((result) => {
                    this.appUserInfoSubject.next(result);
                    this.log.info(`${this.className} successfully initialized.`, result);
                }),
                catchError((err) => this.handleInitializationError(err)),
            );
    }

    public get(): Observable<AppUserInfo> {

        return this.appUserInfoSubject.asObservable();
    }

    private handleInitializationError(err: any): Observable<any> {
        const message = `${this.className}.init() failed`;
        this.log.error(message, err);
        this.appUserInfoSubject.error(message);
        return err;
    }

}
