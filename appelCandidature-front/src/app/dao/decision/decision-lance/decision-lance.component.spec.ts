import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DecisionLanceComponent } from './decision-lance.component';

describe('DecisionLanceComponent', () => {
  let component: DecisionLanceComponent;
  let fixture: ComponentFixture<DecisionLanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DecisionLanceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DecisionLanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
