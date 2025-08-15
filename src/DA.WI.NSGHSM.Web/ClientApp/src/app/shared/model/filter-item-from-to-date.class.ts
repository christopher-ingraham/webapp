import { FilterItem } from './filter-item.class';
import { FromToDate } from './from-to-date.class';

export class FilterItemFromToDate extends FilterItem<FromToDate> {
    constructor(value = new FromToDate(), isEnabled = false) {
        super(value, isEnabled);
    }
    public get search(): string {
        if (this.isEnabled) {
            return this.value.from.toISOString() + '|' + this.value.to.toISOString();
        }
        return null;
    }
    public get searchFrom(): string {
        if (this.isEnabled) {
            return this.value.from.toISOString();
        }
        return null;
    }
    public get searchTo(): string {
        if (this.isEnabled) {
            return this.value.to.toISOString();
        }
        return null;
    }
    public set(fromDate: Date, toDate: Date) {
        this.value.from = fromDate;
        this.value.to = toDate;
    }
}
