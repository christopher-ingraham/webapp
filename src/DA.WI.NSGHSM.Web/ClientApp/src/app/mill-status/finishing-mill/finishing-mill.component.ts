import { Component, OnInit, OnDestroy } from '@angular/core';

import { FinishingMillApiService, FinishingMillData } from '@app/shared';

@Component({
    selector: 'app-finishing-mill',
    templateUrl: './finishing-mill.component.html',
    styleUrls: ['./finishing-mill.component.scss']
})
export class FinishingMillComponent implements OnInit, OnDestroy {

    public data: FinishingMillData;
    public color: string;
    protected interval: any;
    constructor(
        private finishingMillService: FinishingMillApiService
    ) { }

    loadData() {
        this.finishingMillService.readFromListUrl().subscribe(x => this.data = x);
    }

    protected getColor(value: number, mis: boolean) {
        if (mis) {
            switch (value) {
                case 0:
                    this.color = '2px solid red';
                    break;
                case 1:
                    this.color = '2px solid green';
                    break;
                default:
                    this.color = '';
                    break;
            }
        } else { this.color = ''; }

        return this.color;
    }

    ngOnInit() {
        this.interval = setInterval(() => {
            this.loadData();
          }, 2000);
        this.loadData();
    }
    ngOnDestroy() {
        clearInterval(this.interval);
    }
}
