import { Component, OnInit } from '@angular/core';
import {NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import { IHours } from '../class/IDate';

@Component({
  selector: 'ngbd-datepicker-popup',
  templateUrl: './ngbd-datepicker-popup.component.html',
  styleUrls: ['./ngbd-datepicker-popup.component.css']
})
export class NgbdDatepickerPopupComponent implements OnInit {

  dateStart: NgbDateStruct;
  dateEnd: NgbDateStruct;
  timeStart: IHours;
  timeEnd: IHours;
  step : IHours = {hour:1, minute: 15, second: 30}

  constructor() { }

  ngOnInit() {
  }

}
