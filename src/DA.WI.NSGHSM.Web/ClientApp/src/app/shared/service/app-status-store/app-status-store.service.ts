import { Injectable } from '@angular/core';
import { Store } from 'rxjs-observable-store';

import { AppStatus } from './model';

@Injectable({
  providedIn: 'root'
})
export class AppStatusStoreService
    extends Store<AppStatus> {

    constructor() {
        super(new AppStatus());
    }

    public get isLoggedIn(): boolean {
        return this.state.isLoggedIn;
    }
    public set isLoggedIn(value: boolean) {
        this.setState({
            ...this.state,
            isLoggedIn: value,
        });
    }

    public get isUomSI(): boolean {
        return this.state.isUomSI;
    }
    public set isUomSI(value: boolean) {
        this.setState({
            ...this.state,
            isUomSI: value,
        });
    }

    public get localeId(): string {
        return this.state.localeId;
    }
    public set localeId(value: string) {
        this.setState({
            ...this.state,
            localeId: value,
        });
    }

}
