import { Injectable } from '@angular/core';
import { DatePipe } from '@angular/common';
import { classToPlain } from 'class-transformer';

import { LogEntry, LogEntryList, LogType } from './models';

import { environment } from '@app/environments';

@Injectable({
    providedIn: 'root'
})
export class LogService {

    private logEntries: LogEntryList;

    public stringifySpace: string | number = 2;
    public level: LogType = environment.production ? LogType.Warning : LogType.Debug;
    public verbose: boolean = false;

    constructor(private datePipe: DatePipe) {
        this.logEntries = new LogEntryList();
        this.info(`New ${LogService['name']} constructed.`);
    }

    public error(message?: any, ...optionalParams: any[]): void {
        const entry = LogEntry.error(message, optionalParams);
        this.logAndAppend(console.error, entry);
    }

    public info(message?: any, ...optionalParams: any[]): void {
        const entry = LogEntry.info(message, optionalParams);
        this.logAndAppend(console.info, entry);
    }

    public warn(message?: any, ...optionalParams: any[]): void {
        const entry = LogEntry.warn(message, optionalParams);
        this.logAndAppend(console.warn, entry);
    }

    public debug(message?: any, ...optionalParams: any[]): void {
        const entry = LogEntry.debug(message, optionalParams);
        // console.debug is an alias of console.log
        // tslint:disable-next-line
        this.logAndAppend(console.debug, entry);
    }

    public getLogEntries(): Partial<LogEntry>[] {
        return this.logEntries.map((entry) =>
            Object.assign(classToPlain(entry), { message: this.buildOutputMessage(entry) })
        );
    }

    private buildOutputMessage(entry: LogEntry) {
        const timestamp = this.datePipe.transform(entry.timestamp, 'HH:mm:ss.SSSS');
        const level = LogType[entry.type].toUpperCase();
        return `MH | ${timestamp} | ${level} | ${(entry.message)}`;
    }

    private anyListToStringList(...optionalParams: any[]): string[] {
        const lines: string[] = [];
        const params = optionalParams[0];

        for (let paramIndex = 0; (paramIndex < params.length); paramIndex++) {
            const param = params[paramIndex];
            const paramText = JSON.stringify(param, null, this.stringifySpace);
            const paramLines = (typeof paramText === 'undefined') ? [`${param}`] : paramText.split(/\r?\n/);

            const pad = (paramLines.length < 10) ? 2 : 3;
            for (let paramLineIndex = 0; (paramLineIndex < paramLines.length); paramLineIndex++) {
                const lineNumber = `${1 + paramLineIndex}`.padStart(pad);
                lines.push(`[${1 + paramIndex}.${lineNumber}] ${paramLines[paramLineIndex]}`);
            }
        }

        return lines;
    }

    private logAndAppend(logger: (message?: string, ...optionalParams: any[]) => void, entry: LogEntry) {
        if (entry.type <= this.level) {
            // ---  log
            const outputMessage = this.buildOutputMessage(entry);
            if (this.verbose) {
                let outputLines: string[] = [];

                if (entry.optionalParams.length) {
                    outputLines = this.anyListToStringList(entry.optionalParams).map((line) => `${outputMessage} | ${line}`);
                } else {
                    outputLines.push(outputMessage);
                }
                outputLines.forEach((line) => logger(line));
            } else {
                logger(outputMessage, ...entry.optionalParams);
            }
            // ---  append
            // this.logEntries.append(entry);
        }
    }

}
