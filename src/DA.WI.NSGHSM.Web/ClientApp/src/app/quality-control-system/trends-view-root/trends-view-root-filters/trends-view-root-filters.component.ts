import { Component, OnInit, Input } from '@angular/core';
import { Subject } from 'rxjs';

import { RepHmPieceListFilterFromRoot } from '@app/shared';


@Component({
    selector: 'app-trends-view-root-filters',
    templateUrl: './trends-view-root-filters.component.html',
    styleUrls: ['./trends-view-root-filters.component.css']
})
export class TrendsViewRootFiltersComponent implements OnInit {

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
