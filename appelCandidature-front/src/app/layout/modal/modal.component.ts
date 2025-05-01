import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-modal',
  imports: [
    CommonModule
  ],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.scss'
})
export class ModalComponent {
  @Input() title = '';
  @Input() message = '';
  @Input() type: 'success' | 'error' | 'info' = 'info';
  @Input() show = false;

  close() {
    this.show = false;
  }
}
