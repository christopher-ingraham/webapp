import { Injectable } from '@angular/core';
import { NotificationEntry, NotificationEntryLevel, NotificationAppState } from './notification.model';
import { Subject, Observable } from 'rxjs';


@Injectable({
    providedIn: 'root'
})
export class NotifierService {

    private busyCallInProgressCounter: number = 0;
    private lastNotificationSubject = new Subject<NotificationEntry>();
    private lastAppState = new Subject<NotificationAppState>();

    public get isReady(): boolean {
        return (this.busyCallInProgressCounter === 0);
    }

    constructor() {
        this.lastAppState.next(NotificationAppState.Ready);
    }

    busy() {
        if (this.isReady) {
            this.busyCallInProgressCounter = 1;
            this.lastAppState.next(NotificationAppState.Busy);
        } else {
            ++this.busyCallInProgressCounter;
        }
    }

    unbusy() {
        --this.busyCallInProgressCounter;
        if (this.isReady) {
            this.lastAppState.next(NotificationAppState.Ready);
        }
    }

    success(message: string) {
        this.lastNotificationSubject.next({ level: NotificationEntryLevel.Success, message: message });
    }

    info(message: string) {
        this.lastNotificationSubject.next({ level: NotificationEntryLevel.Info, message: message });
    }

    warning(message: string) {
        this.lastNotificationSubject.next({ level: NotificationEntryLevel.Warning, message: message });
    }

    error(message: string) {
        this.lastNotificationSubject.next({ level: NotificationEntryLevel.Error, message: message });
    }

    getAppState(): Observable<NotificationAppState> {
        return this.lastAppState.asObservable();
    }

    getLastNotification(): Observable<NotificationEntry> {
        return this.lastNotificationSubject.asObservable();
    }
}
