import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss',
  animations: [
    trigger('fade', [
      state('visible', style({ opacity: 1, transform: 'scale(1)' })),
      state('hidden', style({ opacity: 0, transform: 'scale(0.95)' })),
      transition('hidden => visible', [animate('200ms ease-out')]),
      transition('visible => hidden', [animate('200ms ease-in')])
    ])
  ]
})
export class AlertComponent {
  @Input() type: 'success' | 'info' | 'warning' | 'danger' | 'gray' = 'info';
  @Input() message: string = '';
  @Input() dismissible: boolean = true;
  @Output() closed = new EventEmitter<void>();

  visible: boolean = true;

  closeAlert() {
    this.visible = false;
    setTimeout(() => this.closed.emit(), 200);
  }

}
