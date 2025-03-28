import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiSelectUsersComponent } from './multi-select-users.component';

describe('MultiSelectUsersComponent', () => {
  let component: MultiSelectUsersComponent;
  let fixture: ComponentFixture<MultiSelectUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MultiSelectUsersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiSelectUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
