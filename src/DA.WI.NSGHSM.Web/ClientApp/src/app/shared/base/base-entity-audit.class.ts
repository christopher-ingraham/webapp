export class BaseEntityAudit {
    constructor(
        public operator: string,
        public revision: Date = new Date()) { }
}
