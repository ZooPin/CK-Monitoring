import { Component, OnInit } from '@angular/core';
import { LogSearcherService } from '../log-searcher.service';
import { LogType, ILogView } from '../class/ILogView';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-log-searcher',
  templateUrl: './log-searcher.component.html',
  styleUrls: ['./log-searcher.component.css']
})
export class LogSearcherComponent implements OnInit {

  data : Array<ILogView>;

  constructor(private LogSearcherService: LogSearcherService) { }

  ngOnInit() {
    this.GetAllLog(20);
  }

  async Search(query:string) {
    await this.LogSearcherService.Search(query).subscribe(data => this.data = data as Array<ILogView>);
  }

  async GetAllLog(maxLogToReturn:number) {
    await this.LogSearcherService.GetAllLog(maxLogToReturn).subscribe(data => this.data = data as Array<ILogView>);
  }
}
