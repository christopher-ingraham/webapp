export class FromToDate {
    constructor(
        public from: Date = new Date(),
        public to: Date = new Date()
    ) { }
    public get asArray(): Date[] {
        return [this.from, this.to];
    }
}
