import { Component } from '@angular/core';
import { Http, HttpModule } from '@angular/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  apiValues: string[] = [];

  constructor(private _http: Http) {  }

  ngOnInit() {
    this._http.get('api/test').subscribe(values => this.apiValues = values.json() as string[]);
  }

}
