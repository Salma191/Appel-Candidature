import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DecisionDraftComponent } from './decision-draft.component';

describe('DecisionDraftComponent', () => {
  let component: DecisionDraftComponent;
  let fixture: ComponentFixture<DecisionDraftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DecisionDraftComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DecisionDraftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
