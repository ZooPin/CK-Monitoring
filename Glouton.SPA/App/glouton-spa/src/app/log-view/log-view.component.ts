import { Component, OnInit, Input } from '@angular/core';
import { LogType, ILogView } from '../class/ILogView';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-log-view',
  templateUrl: './log-view.component.html',
  styleUrls: ['./log-view.component.css']
})
export class LogViewComponent implements OnInit {

  public ExceptionCollapsed : boolean = true;
  public InnerExceptionCollapsed : boolean = true;
  public LogCollapsed: boolean = true;

  constructor() { }

  ngOnInit() {
  }

  @Input('log') log : ILogView;
}
