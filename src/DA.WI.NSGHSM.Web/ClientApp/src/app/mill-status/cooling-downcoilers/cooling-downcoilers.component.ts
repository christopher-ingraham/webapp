import { Component, OnInit, OnDestroy } from '@angular/core';

import { CoolingDowncoilersApiService, CoolingDowncoilersData } from '@app/shared';

@Component({
    selector: 'app-cooling-downcoilers',
    templateUrl:  './cooling-downcoilers.component.html',
    styleUrls: ['./cooling-downcoilers.component.scss']
})
export class CoolingDowncoilersComponent implements OnInit, OnDestroy {

    public data: CoolingDowncoilersData;
    public color: string;
    protected interval: any;
    constructor(
        private coolingDowncoilersService: CoolingDowncoilersApiService
    ) { }

    loadData() {
        this.coolingDowncoilersService.readFromListUrl().subscribe(x => this.data = x);
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
