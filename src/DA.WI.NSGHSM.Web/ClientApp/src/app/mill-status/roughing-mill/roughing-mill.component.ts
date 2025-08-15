import { Component, OnInit, OnDestroy } from '@angular/core';

import { RoughingMillApiService, RoughingMillData } from '@app/shared';

@Component({
    selector: 'app-roughing-mill',
    templateUrl: './roughing-mill.component.html',
    styleUrls: ['./roughing-mill.component.scss']
})
export class RoughingMillComponent implements OnInit, OnDestroy {

    public data: RoughingMillData;
    public myStyles;
    public color: string;

    protected interval: any;
    constructor(
        private roughingMillService: RoughingMillApiService
    ) { }

    loadData() {
        this.roughingMillService.readFromListUrl().subscribe(x => this.data = x);
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
