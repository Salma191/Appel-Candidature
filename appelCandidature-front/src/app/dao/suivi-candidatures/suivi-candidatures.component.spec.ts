import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuiviCandidaturesComponent } from './suivi-candidatures.component';

describe('SuiviCandidaturesComponent', () => {
  let component: SuiviCandidaturesComponent;
  let fixture: ComponentFixture<SuiviCandidaturesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuiviCandidaturesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuiviCandidaturesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
