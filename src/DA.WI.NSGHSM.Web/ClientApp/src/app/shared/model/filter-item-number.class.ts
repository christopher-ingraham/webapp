import { FilterItem } from './filter-item.class';

export class FilterItemNumber extends FilterItem<number> {
    constructor(value: number = 0, isEnabled = false) {
        super(value, isEnabled);
    }
    public get search(): string {
        return this.isEnabled ? '' + this.value : null;
    }
}
