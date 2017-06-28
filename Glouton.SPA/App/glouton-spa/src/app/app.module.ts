import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';


import { AppComponent } from './app.component';
import { LogSearcherComponent } from './log-searcher/log-searcher.component';
import { LogSearcherService } from './log-searcher.service';
import { LogViewComponent } from './log-view/log-view.component';

@NgModule({
  declarations: [
    AppComponent,
    LogSearcherComponent,
    LogViewComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    NgbModule.forRoot()
  ],
  providers: [LogSearcherService],
  bootstrap: [AppComponent]
})
export class AppModule { }
