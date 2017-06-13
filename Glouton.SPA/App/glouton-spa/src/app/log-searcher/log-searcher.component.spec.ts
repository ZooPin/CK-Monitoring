import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogSearcherComponent } from './log-searcher.component';

describe('LogSearcherComponent', () => {
  let component: LogSearcherComponent;
  let fixture: ComponentFixture<LogSearcherComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogSearcherComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogSearcherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
