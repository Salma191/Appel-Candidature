import { Component } from '@angular/core';
import { SidebarService } from '../../services/sidebar.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-card',
  imports: [
    CommonModule
  ],
  templateUrl: './card.component.html',
  styleUrl: './card.component.scss'
})
export class CardComponent {

  constructor(private sidebarService: SidebarService) { }
  
    isSidebarOpen: boolean = false;
  
  
    ngOnInit() {
      this.sidebarService.isSidebarOpen$.subscribe((isOpen) => {
        this.isSidebarOpen = isOpen;
      });
    }
}
