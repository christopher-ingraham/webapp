import { OnDestroy, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { LogService } from '@app/core';
import { SubscriptionList } from '../utility';

export abstract class BaseEntityListFilterComponent<TEntityListFilter>
    extends SubscriptionList implements OnDestroy {
    public form: FormGroup;

    @Output() filterChange = new EventEmitter<TEntityListFilter>();

    constructor(protected log: LogService) {
        super();
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }

}
