export abstract class EntityListFilter<TEntity> {

    public clear() {
        // null all own properties
        Object.getOwnPropertyNames(this).forEach((propertyName) =>
            this[propertyName] = null
        );
    }

    constructor(options?: Partial<TEntity>) {
        this.clear();
        // optionally initialize some properties
        if (options) {
            Object.assign(this, options);
        }
    }
}
