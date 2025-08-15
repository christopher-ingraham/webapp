import { Subscription } from 'rxjs';

export abstract class SubscriptionList {
    private subscriptions: Subscription[] = [];

    protected subscribe(subscription: Subscription, ...subscriptions: Subscription[]) {
        this.subscriptions.push(subscription);
        if (subscriptions.length) {
            this.subscriptions.push(...subscriptions);
        }
    }

    protected unsubscribeAll() {
        this.subscriptions.forEach((subscription) => subscription.unsubscribe());
    }
}
