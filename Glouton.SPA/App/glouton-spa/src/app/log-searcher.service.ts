import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';


@Injectable()
export class LogSearcherService {

  private _route: string;
  private _port: number;

  constructor(private _http: Http) { }

  Init(route: string, port: number) {
    this._route = route;
    this._port = port;
  }

  Search (query:string) {
    let path: string = "/api/search/query/" + query
    return this._http.get(path).map(res => res.json());
  }

  GetAllLog(maxLogToReturn:number) {
    let path: string = "/api/search/all/" + maxLogToReturn;
    return this._http.get(path).map(res => res.json());
  }

  private GeneratePath () : string {
    return "http://" + this._route + ":" + this._port + "/";
  }

}
