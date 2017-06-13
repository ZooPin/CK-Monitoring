import {ILogView, LogType} from '../class/ILogView'

export class CloseGroup implements ILogView
{
    logType: LogType;
    exception: string;
    logTime: string;
    logLevel: string;
}