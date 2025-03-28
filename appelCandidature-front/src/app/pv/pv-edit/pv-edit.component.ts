import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pv-edit',
  imports: [
    CommonModule
  ],
  templateUrl: './pv-edit.component.html',
  styleUrl: './pv-edit.component.scss'
})
export class PvEditComponent {
    @Input() isOpen = false;
    @Input() title = '';
    @Output() closed = new EventEmitter<void>();
    @Output() confirmed = new EventEmitter<void>();
    @Input() confirmText: string = '';

    ngOnInit(): void {
        this.isOpen = true;
    }

    close() {
        this.closed.emit();
    }

    openModal() {
        this.isOpen = true;
    }

    closeModal() {
        this.isOpen = false;
    }

    onConfirm() {
        console.log('Action confirmée');
        this.isOpen = false;
    }

    confirm() {
        this.confirmed.emit();
    }

    fileName: string | null = null;

    onFileSelected(event: any) {
      const file = event.target.files[0];
      
      if (file) {
          this.fileName = file.name;
          event.target.value = ''; // Réinitialiser l'input pour permettre une nouvelle sélection
      }
  }
  



}
