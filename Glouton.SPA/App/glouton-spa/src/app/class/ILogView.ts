export enum LogType {
    Opengroup,
    Line,
    CloseGroup
}

export interface ILogView {
    logType: LogType;
    exception: string;
    logTime: string;
    logLevel :string;
}