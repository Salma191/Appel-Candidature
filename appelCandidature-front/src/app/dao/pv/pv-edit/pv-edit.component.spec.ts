import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PvEditComponent } from './pv-edit.component';

describe('PvEditComponent', () => {
  let component: PvEditComponent;
  let fixture: ComponentFixture<PvEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PvEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PvEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
