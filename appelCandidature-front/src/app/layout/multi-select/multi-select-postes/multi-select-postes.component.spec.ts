import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiSelectPostesComponent } from './multi-select-postes.component';

describe('MultiSelectComponent', () => {
  let component: MultiSelectPostesComponent;
  let fixture: ComponentFixture<MultiSelectPostesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MultiSelectPostesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiSelectPostesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
