import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PjComponent } from './pj.component';

describe('PjComponent', () => {
  let component: PjComponent;
  let fixture: ComponentFixture<PjComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PjComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
