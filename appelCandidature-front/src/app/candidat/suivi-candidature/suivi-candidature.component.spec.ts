import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuiviCandidatureComponent } from './suivi-candidature.component';

describe('SuiviCandidatureComponent', () => {
  let component: SuiviCandidatureComponent;
  let fixture: ComponentFixture<SuiviCandidatureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuiviCandidatureComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuiviCandidatureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
