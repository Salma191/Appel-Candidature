import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-piece-jointe-upload',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './piece-jointe-upload.component.html',
  styleUrl: './piece-jointe-upload.component.scss'
})
export class PieceJointeUploadComponent {

  @Input() isOpen = false;
@Input() candidatureId!: number;

@Output() confirmed = new EventEmitter<any>();
@Output() closed = new EventEmitter<void>();

pieceJointe = {
  fichier: '',
  typeDoc: null
};

onFileSelected(event: Event): void {
  const file = (event.target as HTMLInputElement)?.files?.[0];
  if (file) {
    const reader = new FileReader();
    reader.onload = () => {
      this.pieceJointe.fichier = reader.result as string; // Base64
    };
    reader.readAsDataURL(file);
  }
}

confirmUpload(): void {
  if (this.pieceJointe.fichier && this.pieceJointe.typeDoc) {
    this.confirmed.emit({
      ...this.pieceJointe,
      candidatureId: this.candidatureId
    });
    this.close();
  }
}

close(): void {
  this.isOpen = false;
  this.closed.emit();
}

}
