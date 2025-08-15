export class ReportListItem {
    constructor (
        public readonly id: number,
        public readonly label: string,
    ) {
        this.label = label + ' Report';
    }

    public get htmlId(): string {
        return `id-${this.id}-${this.label}`.replace(/\s+/g, '-').toLowerCase();
    }
}
