import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PvAddComponent } from './pv-add.component';

describe('PvAddComponent', () => {
  let component: PvAddComponent;
  let fixture: ComponentFixture<PvAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PvAddComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PvAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
