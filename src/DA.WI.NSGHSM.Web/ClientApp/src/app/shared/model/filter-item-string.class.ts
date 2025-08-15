import { FilterItem } from './filter-item.class';

export class FilterItemString extends FilterItem<string> {
    constructor(value = '', isEnabled = false) {
        super(value, isEnabled);
    }
    public get search(): string {
        return this.isEnabled ? this.value : null;
    }
    public get searchLike(): string {
        return this.isEnabled ? `%${this.value}%` : null;
    }
}
