import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificationCandidaturesComponent } from './verification-candidatures.component';

describe('VerificationCandidaturesComponent', () => {
  let component: VerificationCandidaturesComponent;
  let fixture: ComponentFixture<VerificationCandidaturesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VerificationCandidaturesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerificationCandidaturesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
