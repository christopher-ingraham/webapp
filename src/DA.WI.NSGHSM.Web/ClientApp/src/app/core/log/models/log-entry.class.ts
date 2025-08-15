import { LogType } from './log-type.enum';

export class LogEntry {
    public readonly timestamp: Date;

    constructor(
        public readonly type: LogType,
        public readonly message: string = '',
        public readonly optionalParams?: any[]
    ) {
        this.timestamp = new Date();
    }

    public static error(message?: string, optionalParams?: any[]) {
        return new LogEntry(LogType.Error, message, optionalParams);
    }

    public static warning(message?: string, optionalParams?: any[]) {
        return new LogEntry(LogType.Warning, message, optionalParams);
    }

    public static warn(message?: string, optionalParams?: any[]) {
        return LogEntry.warning(message, optionalParams);
    }

    public static info(message?: string, optionalParams?: any[]) {
        return new LogEntry(LogType.Info, message, optionalParams);
    }

    public static debug(message?: string, optionalParams?: any[]) {
        return new LogEntry(LogType.Debug, message, optionalParams);
    }
}
