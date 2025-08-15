import { Component, OnInit, Input } from '@angular/core';
import { Subject } from 'rxjs';

import { RepHmPieceListFilterFromRoot } from '@app/shared';

@Component({
    selector: 'app-main-signals-root-filters',
    templateUrl: './main-signals-root-filters.component.html',
    styleUrls: ['./main-signals-root-filters.component.css']
})
export class MainSignalsRootFiltersComponent implements OnInit {

    @Input() public filters: Subject<RepHmPieceListFilterFromRoot>;

    public filter: RepHmPieceListFilterFromRoot;

    public readonly modelOptions = { standalone: true };

    constructor() {
        this.filter = new RepHmPieceListFilterFromRoot();
    }

    ngOnInit(): void {
    }

    public applyFilters(event: KeyboardEvent) {
        this.filters.next(this.filter);
    }

}
