import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import { SearcherService } from '../searcher.service';
import { IFilter } from '../class/IFilter';

@Component({
  selector: 'filter-checkbox',
  templateUrl: './filter-checkbox.component.html',
  styleUrls: ['./filter-checkbox.component.css']
})
export class FilterCheckboxComponent implements OnInit {
  
  public checkboxGroupForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private searcher: SearcherService) {}

  updateService(): void {
    setTimeout(() => this.searcher.Filter = this.checkboxGroupForm.value as IFilter, 1);
  }

  ngOnInit() {
    this.checkboxGroupForm = this.formBuilder.group({
      trace: false,
      info: false,
      warn: false,
      error: false,
      fatal: false,
      debug: false
    });
    this.updateService();
  }
}
