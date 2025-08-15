import { Subject, PartialObserver } from 'rxjs';

export class EntitySelectionHelper<TEntity> {
    private selectedEntity?: TEntity;
    private clickedEntity: Subject<TEntity>;

    public get isSelected(): boolean {
        return this.selectedEntity ? true : false;
    }

    public get entity(): TEntity {
        return this.selectedEntity;
    }
    public set entity(value: TEntity) {
        this.selectedEntity = value;
        this.broadcast();
    }

    public get subject(): Subject<TEntity> {
        return this.clickedEntity;
    }

    constructor() {
        this.clickedEntity = new Subject<TEntity>();
    }

    public subscribe() {
        return this.clickedEntity.subscribe((entity: TEntity) => {
            this.selectedEntity = entity;
        });
    }

    public broadcast() {
        if (this.clickedEntity) {
            this.clickedEntity.next(this.selectedEntity);
        }
    }
    public broadcastClear() {
        if (this.clickedEntity) {
            this.clickedEntity.next();
        }
    }
}
