import { LogEntry } from './log-entry.class';

export class LogEntryList {

    private readonly items: LogEntry[];

    constructor(public maxLength: number = 50) {
        this.items = [];
    }

    public append(item: LogEntry) {
        this.items.push(item);
        if (this.items.length > this.maxLength) {
            this.items.shift();
        }
    }

    public get map() { return this.items.map; }
}
