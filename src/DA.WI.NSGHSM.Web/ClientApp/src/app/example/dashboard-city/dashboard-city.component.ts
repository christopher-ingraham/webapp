import { Component, OnInit, Input } from '@angular/core';

import { ListRequest } from '@app/core';

import { MeasurementApiService } from '../measurement/measurement-api.service';
import { MeasurementListFiltering, MeasurementListItem } from '../measurement/measurement.model';

@Component({
  selector: 'app-dashboard-city',
  templateUrl: './dashboard-city.component.html'
})
export class DashboardCityComponent implements OnInit {

// tslint:disable-next-line: no-input-rename
  @Input('city-id') cityId: number;

  data: MeasurementListItem[] = [];
  temperatures: number[] = [];
  cityName: string;

  constructor(private measurementApiService: MeasurementApiService) { }

  ngOnInit() {
    const listFilter: ListRequest<MeasurementListFiltering> = {
      filter: {
        cityId: this.cityId
      },
      page: {
        skip: 0,
        take: 10
      },
      sort: {
        items: [
          {
            fieldName: 'MeasuredAt',
            isDescending: true
          }
        ]
      }
    };

    this.measurementApiService.readList(listFilter).subscribe(_ => {
      this.data = _.data;
      this.data.forEach(val => this.temperatures.push(val.temperatureC));
      this.cityName = this.data[0].cityName;
    });
  }

}
