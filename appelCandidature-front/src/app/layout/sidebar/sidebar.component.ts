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

  toggleMenu(menu: string) {
    if (menu === 'projects') {
      this.isProjectsMenuOpen = !this.isProjectsMenuOpen;
    } else if (menu === 'team') {
      this.isTeamMenuOpen = !this.isTeamMenuOpen;
    }
  }

}