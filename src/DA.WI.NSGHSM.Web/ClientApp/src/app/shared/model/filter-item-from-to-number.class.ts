import { FromToNumber } from './from-to-number.class';
import { FilterItem } from './filter-item.class';

export class FilterItemFromToNumber extends FilterItem<FromToNumber> {

    constructor(value = new FromToNumber(), isEnabled = false) {
        super(value, isEnabled);
    }

    public get search(): string {
        if (this.isEnabled) {
            return this.value.from.toString() + '|' + this.value.to.toString();
        }
        return null;
    }
    public get searchFrom(): string {
        if (this.isEnabled) {
            return this.value.from.toString();
        }
        return null;
    }
    public get searchTo(): string {
        if (this.isEnabled) {
            return this.value.to.toString();
        }
        return null;
    }
}
