import { Injectable } from '@angular/core';
import { IHour, IDate } from './class/IDate';
import { IFilter } from './class/IFilter';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';


@Injectable()
export class SearcherService {

  private _dateStart: IDate;
  private _dateEnd: IDate;
  private _hourStart: IHour;
  private _hourEnd: IHour;
  private _filter: IFilter;
  private _query: string;
  private _appId: string;
  private _monitorId: string;

  constructor(private _http: Http) { }

  Send () {
    this._http.get(this.ConstructQuery()).map(res => res.json());
  }

  private ConstructQuery() : string {
    if(!this.IsSendable) throw new TypeError("Object not sendable");
    //model: {dateStart}/{dateEnd}/{monitorId}/{appId}/{fields}/{keyword}?[logLevel=...]
    let query:string = "/api/search/";
    query += this.ConstructDate(this._dateStart, this._hourStart);
    query += "/" + this.ConstructDate(this._dateEnd, this._hourEnd);
    query += "/" + (this._monitorId) ? this._monitorId : "*";
    query += "/" + (this._appId) ? this._appId : "*";
    query += "/*";
    query += "/" + (this._query) ? this._query : "*";
    query += "?"
    this.ConstructFilter(this._filter).forEach((item, index) => {
       query += (index == 0) ? "" : "&";
       query += "logLevel=" + item;
    });
    return query;
  }

  private ConstructDate(date:IDate, hour:IHour) {
    // model: {year}-{month}-{day}T{hour}:{minute}:{second}.{milisecond}Z
    let dateSend:string = "";
    dateSend += date.year + "-";
    dateSend += date.month + "-";
    dateSend += date.day + "T";
    dateSend += hour.hour + ":";
    dateSend += hour.minute + ":";
    dateSend += hour.second + ".000Z";
    return dateSend;
  }

  private ConstructFilter(filter: IFilter) : Array<string> {
    let filterArray: Array<string> = [];
    if(filter.trace) filterArray.push("trace");
    if(filter.error) filterArray.push("error");
    if(filter.info) filterArray.push("info");
    if(filter.warn) filterArray.push("warn");
    if(filter.fatal) filterArray.push("fatal");
    if(filter.debug) filterArray.push("debug");
    return filterArray;
  }

  get IsSendable() {
    return this._dateStart &&
           this._dateEnd &&
           this._hourStart &&
           this._hourEnd;
  }

  get DateStart() {
    return this._dateStart;
  }
  set DateStart(dateStart: IDate) {
    this._dateStart = dateStart;
  }

  get DateEnd() {
    return this._dateEnd;
  }
  set DateEnd(dateEnd: IDate) {
    this._dateEnd = dateEnd;
  }

  get HourStart() {
    return this._hourStart;
  }
  set HourStart(hour: IHour) {
    this._hourStart = hour;
  }

  get HourEnd() {
    return this._hourEnd;
  }
  set HourEnd(hour: IHour) {
    this._hourEnd = hour;
  }

  get Filter() {
    return this._filter;
  }
  set Filter(filter: IFilter) {
    this._filter = filter;
  }

  get Query() {
    return this._query;
  }
  set Query(query: string) {
    this._query = query;
  }

  get AppId() {
    return this._appId;
  }
  set AppId(appId: string){
    this._appId = appId;
  }

  get MonitorId() {
    return this._monitorId;
  }
  set MonitorId(monitorId: string) {
    this._monitorId = monitorId;
  }
}
