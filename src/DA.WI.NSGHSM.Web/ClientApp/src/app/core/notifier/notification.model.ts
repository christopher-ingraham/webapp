export interface NotificationEntry {

    level: NotificationEntryLevel;
    message: string;

}

export enum NotificationEntryLevel {

    Success,
    Info,
    Warning,
    Error
}

export enum NotificationAppState {

    Ready,
    Busy
}
