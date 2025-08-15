import { Component, OnInit, OnDestroy } from '@angular/core';
import { MediaChange, MediaObserver } from '@angular/flex-layout';
import { Subscription } from 'rxjs';

import { NotificationService } from '@progress/kendo-angular-notification';

import { NotifierService, NotificationEntry, NotificationEntryLevel } from '@app/core';
import { SubscriptionList } from '@app/shared';

@Component({
    selector: 'app-shell',
    templateUrl: './shell.component.html'
})
export class ShellComponent extends SubscriptionList implements OnInit, OnDestroy {

    public sidebarIsVisible: Boolean = true;

    constructor(
        private notifier: NotifierService,
        private notificationPopup: NotificationService,
        private mediaObserver: MediaObserver
    ) {
        super();
    }

    ngOnInit(): void {

        this.subscribe(
            // media$ is deprecated but asObservable is bugged
            // https://github.com/angular/flex-layout/issues/1041
            this.mediaObserver.media$
                .subscribe(_ => this.onScreenSizeChanged(_)));

        this.subscribe(
            this.notifier.getLastNotification()
                .subscribe(_ => this.handleNotification(_))
        );

        this.adjustSidebarVisibility();
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }

    handleNotification(notificationEntry: NotificationEntry): void {

        const notificationStyle = this.getNotificationStyle(notificationEntry.level);

        this.notificationPopup.show({
            content: notificationEntry.message,
            hideAfter: 600,
            position: { horizontal: 'right', vertical: 'bottom' },
            animation: { type: 'fade', duration: 400 },
            type: { style: notificationStyle, icon: true }
        });
    }

    getNotificationStyle(notificationLevel: NotificationEntryLevel): 'none' | 'success' | 'warning' | 'error' | 'info' {

        switch (notificationLevel) {

            case NotificationEntryLevel.Error: return 'error';
            case NotificationEntryLevel.Info: return 'info';
            case NotificationEntryLevel.Warning: return 'warning';
            case NotificationEntryLevel.Success: return 'success';
        }

        return 'none';
    }

    private onScreenSizeChanged(change: MediaChange) {

        this.adjustSidebarVisibility();
    }

    private adjustSidebarVisibility(): void {

        this.sidebarIsVisible = !this.isViewPortOnMobileSize();
    }

    public toggleSidebar(): void {

        this.sidebarIsVisible = !this.sidebarIsVisible;
    }

    public onSidebarLinkClicked(): void {

        if (this.isViewPortOnMobileSize()) {
            this.sidebarIsVisible = false;
        }
    }

    private isViewPortOnMobileSize(): Boolean {

        return this.mediaObserver.isActive('xs');
    }
}
