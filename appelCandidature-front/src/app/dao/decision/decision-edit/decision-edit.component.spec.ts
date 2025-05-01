import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DecisionEditComponent } from './decision-edit.component';

describe('DecisionEditComponent', () => {
  let component: DecisionEditComponent;
  let fixture: ComponentFixture<DecisionEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DecisionEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DecisionEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
