import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SidebarService } from '../../sidebar.service';
import { RouterModule } from '@angular/router';



@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [ 
    CommonModule,
    RouterModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SidebarComponent{

  isProjectsMenuOpen: boolean = false;
  isTeamMenuOpen: boolean = false;

  constructor(private sidebarService: SidebarService) { }

  toggleMenu(menu: string) {
    if (menu === 'projects') {
      this.isProjectsMenuOpen = !this.isProjectsMenuOpen;
    } else if (menu === 'team') {
      this.isTeamMenuOpen = !this.isTeamMenuOpen;
    }
  }


  isSidebarOpen: boolean = false; // Déclarez la propriété ici


  ngOnInit(): void {
    // Abonnez-vous à l'état du menu dans le service
    this.sidebarService.isSidebarOpen$.subscribe((isOpen) => {
      this.isSidebarOpen = isOpen;
    });
  }

  toggleSidebar(): void {
    this.sidebarService.toggleSidebar(); // Change l'état du menu
  }


}