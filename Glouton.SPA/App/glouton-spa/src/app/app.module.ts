import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';

import { AppComponent } from './app.component';
import { LogSearcherComponent } from './log-searcher/log-searcher.component';
import { LogSearcherService } from './log-searcher.service';
import { SearcherService } from './searcher.service';
import { LogViewComponent } from './log-view/log-view.component';
import { NgbdDatepickerPopupComponent } from './ngbd-datepicker-popup/ngbd-datepicker-popup.component';
import { FilterCheckboxComponent } from './filter-checkbox/filter-checkbox.component';

@NgModule({
  declarations: [
    AppComponent,
    LogSearcherComponent,
    LogViewComponent,
    NgbdDatepickerPopupComponent,
    FilterCheckboxComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    NgbModule.forRoot(),
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [LogSearcherService, SearcherService],
  bootstrap: [AppComponent]
})
export class AppModule { }
