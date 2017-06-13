import { Component, OnInit } from '@angular/core';
import { LogSearcherService } from '../log-searcher.service';
import { LogType } from '../class/ILogView';

@Component({
  selector: 'app-log-searcher',
  templateUrl: './log-searcher.component.html',
  styleUrls: ['./log-searcher.component.css']
})
export class LogSearcherComponent implements OnInit {

  data : any;

  constructor(private LogSearcherService: LogSearcherService) { }

  ngOnInit() {
  }

  async Search(query:string) {
    await this.LogSearcherService.Search(query).subscribe(data => this.data = data);
  }
}
