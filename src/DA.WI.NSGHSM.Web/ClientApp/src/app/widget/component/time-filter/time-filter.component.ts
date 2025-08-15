import { Component, EventEmitter, OnInit, Output, Input } from '@angular/core';

import * as moment from 'moment';
import { FromToDate } from '../../../shared/model/from-to-date.class';


@Component({
    selector: 'app-time-filter',
    templateUrl: './time-filter.component.html',
    styleUrls: ['./time-filter.component.css']
})
export class TimeFilterComponent implements OnInit {
    @Input() public disabled = false;
    @Output() public timeChanged = new EventEmitter<Date[]>();

    @Input() public dateTimeFrom: Date;
    @Output() public dateTimeFromChange = new EventEmitter<Date>();

    @Input() public dateTimeTo: Date;
    @Output() public dateTimeToChange = new EventEmitter<Date>();

    @Input() public thisShift: FromToDate;
    @Input() public lastShift: FromToDate;

    public get value1(): Date {
        return this.dateTimeFrom;
    }
    public set value1(value: Date) {
        this.dateTimeFrom = value;
        this.dateTimeFromChange.emit(value);
    }

    public get value2(): Date {
        return this.dateTimeTo;
    }
    public set value2(value: Date) {
        this.dateTimeTo = value;
        this.dateTimeToChange.emit(value);
    }

    private get isLastShiftSet(): boolean {
        return this.lastShift ? true : false;
    }
    public get isLastShiftDisabled(): boolean {
        return this.disabled || !this.isLastShiftSet;
    }

    private get isThisShiftSet(): boolean {
        return this.thisShift ? true : false;
    }
    public get isThisShiftDisabled(): boolean {
        return this.disabled || !this.isThisShiftSet;
    }

    public format: string = 'dd/MM/yyyy HH:mm';
    public isManualEditingEnabled: boolean = false;

    constructor() { }

    ngOnInit() {
        this.setRange(
            moment().startOf('day'),
            moment().endOf('day')
        );
    }


    private setRange(d1: moment.Moment, d2: moment.Moment) {
        this.dates = new FromToDate(d1.toDate(), d2.toDate());
    }

    // ---  1  ---

    onTodaySelect(event: MouseEvent) {
        this.setRange(
            moment().startOf('day'),
            moment().endOf('day'));
    }

    onThisWeekSelect(event: MouseEvent) {
        this.setRange(
            moment().startOf('isoWeek'),
            moment().endOf('isoWeek'));
    }

    onThisMonthSelect(event: MouseEvent) {
        this.setRange(
            moment().startOf('month'),
            moment().endOf('isoWeek').subtract(1, 'days'));
    }

    // ---  2  ---

    onYesterdaySelect(event: MouseEvent) {
        this.setRange(
            moment().startOf('day').subtract(1, 'days'),
            moment().endOf('day').subtract(1, 'days'));
    }

    onLastWeekSelect(event: MouseEvent) {
        this.setRange(
            moment().startOf('isoWeek').subtract(1, 'weeks'),
            moment().endOf('isoWeek').subtract(1, 'weeks'));
    }

    onLastMonthSelect(event: MouseEvent) {
        this.setRange(
            moment().startOf('month').subtract(1, 'months'),
            moment().endOf('month').subtract(1, 'months'));
    }

    // ---  3  ---

    onLastShiftSelect(event: MouseEvent) {
        if (this.isLastShiftSet) {
            this.dates = new FromToDate(this.lastShift.from, this.lastShift.to);
        }
    }

    onThisShiftSelect(event: MouseEvent) {
        if (this.isThisShiftSet) {
            this.dates = new FromToDate(this.thisShift.from, this.thisShift.to);
        }
    }

    onManualSelect(event: MouseEvent) {
        this.isManualEditingEnabled = true;
    }

    public get dates(): FromToDate {
        return new FromToDate(this.dateTimeFrom, this.dateTimeTo);
    }
    public set dates(value: FromToDate) {
        this.isManualEditingEnabled = false;
        this.value1 = value.from;
        this.value2 = value.to;
        this.timeChanged.emit(this.dates.asArray);
    }

    getDates(): Date[] {
        return this.dates.asArray;
    }
}
