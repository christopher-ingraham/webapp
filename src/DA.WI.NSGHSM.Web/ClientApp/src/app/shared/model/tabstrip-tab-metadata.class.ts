import { TabstripMetadata } from './tabstrip-metadata.class';
import { TabstripTabOptions, TabstripTabType } from './tabstrip-tab-options.class';

export class TabstripTabMetadata<TEntityId> {
    public disabled: boolean = false;

    public get isSelected(): boolean {
        return (this.tabstrip.selected === this.index);
    }

    private _title: string;
    public get title(): string {
        if (this.disabled) {
            return `${this._title} (disabled)`;
        } else {
            return this._title;
        }
    }
    public set title(value: string) {
        this._title = value;
    }

    public readonly type: TabstripTabType;

    public readonly id: TEntityId;
    public readonly isNew: boolean;
    public readonly copyFromId?: TEntityId;

    constructor(
        private readonly tabstrip: TabstripMetadata,
        public index: number,
        options: TabstripTabOptions<TEntityId>
    ) {
        Object.assign(this, options);
    }

    public close() {
        // Tell parent tabstrip I am going away
        this.tabstrip.tabClosed.next({
            entityName: this.type,
            index: this.index,
        });
    }

}
