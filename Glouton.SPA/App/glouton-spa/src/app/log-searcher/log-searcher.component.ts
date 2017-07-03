import { Component, OnInit , NgModule } from '@angular/core';
import { LogSearcherService } from '../log-searcher.service';
import { LogType, ILogView } from '../class/ILogView';
import { SearcherService } from '../searcher.service';
import { NgbModule, NgbDateStruct, NgbDatepicker } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-log-searcher',
  templateUrl: './log-searcher.component.html',
  styleUrls: ['./log-searcher.component.css']
})
export class LogSearcherComponent implements OnInit {

  data : Array<ILogView>;
  model;

  constructor(private LogSearcherService: LogSearcherService, private searcher: SearcherService) { }

  ngOnInit() {
    this.GetAllLog(20);
  }

  async Search(query:string) {
    this.searcher.Query = query;
    await this.searcher.Send().subscribe(data => this.data = data as Array<ILogView>);
  }

  async GetAllLog(maxLogToReturn:number) {
    await this.LogSearcherService.GetAllLog(maxLogToReturn).subscribe(data => this.data = data as Array<ILogView>);
  }
}
