import { Component, OnInit } from '@angular/core';
import {NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import { IHour, IDate } from '../class/IDate';
import { SearcherService } from '../searcher.service';


@Component({
  selector: 'ngbd-datepicker-popup',
  templateUrl: './ngbd-datepicker-popup.component.html',
  styleUrls: ['./ngbd-datepicker-popup.component.css']
})
export class NgbdDatepickerPopupComponent implements OnInit {
  dateStart: NgbDateStruct;
  dateEnd: NgbDateStruct;
  timeStart: IHour;
  timeEnd: IHour;
  step : IHour = {hour:1, minute: 15, second: 30}

  constructor(private searcher: SearcherService) { }

  ngOnInit() {
  }

  updateService(): void {
    this.searcher.DateStart = this.dateStart as IDate;
    this.searcher.DateEnd = this.dateEnd as IDate;
    this.searcher.HourStart = this.timeStart;
    this.searcher.HourEnd = this.timeEnd;
  }

}
