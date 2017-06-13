import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';


import { AppComponent } from './app.component';
import { LogSearcherComponent } from './log-searcher/log-searcher.component';
import { LogSearcherService } from './log-searcher.service';

@NgModule({
  declarations: [
    AppComponent,
    LogSearcherComponent
  ],
  imports: [
    BrowserModule,
    HttpModule
  ],
  providers: [LogSearcherService],
  bootstrap: [AppComponent]
})
export class AppModule { }