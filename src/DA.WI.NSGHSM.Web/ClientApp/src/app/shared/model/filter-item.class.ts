export abstract class FilterItem<TValue> {
    constructor(
        public value: TValue,
        public isEnabled = false) {}
    public abstract get search(): string;
}
