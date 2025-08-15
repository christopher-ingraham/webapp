export abstract class ComboBoxItem<TValue, TLabel> {
    constructor(
        public value: TValue,
        public label: TLabel) {}
}
