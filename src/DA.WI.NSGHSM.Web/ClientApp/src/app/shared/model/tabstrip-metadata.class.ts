import { Subject, BehaviorSubject } from 'rxjs';

import { TabstripTabMetadata } from './tabstrip-tab-metadata.class';
import { TabstripTabOptions } from './tabstrip-tab-options.class';
import { TabstripTabClosedEvent } from './tabstrip-tab-closed-event.interface';

export class TabstripMetadata {
    public readonly tabs: TabstripTabMetadata<any>[];
    public readonly tabSelected: BehaviorSubject<number>;
    public readonly tabClosed: Subject<TabstripTabClosedEvent>;

    public selected = 0;

    constructor(title: string) {
        this.tabs = [];

        this.tabClosed = new Subject<TabstripTabClosedEvent>();
        this.tabClosed.subscribe((event) => this.onTabClosed(event));

        this.tabSelected = new BehaviorSubject<number>(0);

        this.append({ title, type: 'master' });
    }

    public append<TEntityId>(
        options?: TabstripTabOptions<TEntityId>
    ): TabstripTabMetadata<TEntityId> {
        const completeOptions = new TabstripTabOptions<TEntityId>(options);
        const index = this.tabs.length;
        const tabMeta = new TabstripTabMetadata(this, index, completeOptions);
        this.tabs.push(tabMeta);
        this.selected = index;
        return tabMeta;
    }

    public getTabIndexById<TEntityId>(id: TEntityId): number | undefined {
        const candidates = this.tabs.filter((tab) => tab.id === id);
        if (candidates && candidates.length) {
            return candidates[0].index;
        }
        // not found
    }

    private selectTab(tabIndex: number) {
        this.selected = tabIndex;
    }

    private onTabClosed(event: TabstripTabClosedEvent) {
        // Prevent closing master tab
        if (event.index > 0) {
            // Remove tabToCloseIndex-th tab
            this.tabs.splice(event.index, 1);
            // Reindex remaining tabs
            this.tabs.forEach((tab, index) => tab.index = index);
            // Select left tab
            this.selectTab(event.index - 1);
        } else {
            // Select master
            this.selectTab(0);
        }
    }

}
