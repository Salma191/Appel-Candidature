import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PieceJointeUploadComponent } from './piece-jointe-upload.component';

describe('PieceJointeUploadComponent', () => {
  let component: PieceJointeUploadComponent;
  let fixture: ComponentFixture<PieceJointeUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PieceJointeUploadComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PieceJointeUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
